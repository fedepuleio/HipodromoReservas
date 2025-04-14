namespace HipodromoReservas.Tests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        private readonly Mock<IWaitingListService> _waitingListServiceMock;
        private readonly ReservationService _reservationService;

        public ReservationServiceTests()
        {
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _waitingListServiceMock = new Mock<IWaitingListService>();
            _reservationService = new ReservationService(_reservationRepositoryMock.Object, _waitingListServiceMock.Object);
        }

        [Fact]
        public async Task GetReservations_ShouldReturnList()
        {
            // Arrange
            var reservations = new List<Reservation> { new Reservation(DateTime.Today, TimeSlotEnum.SLOT_2330, 1, 1) };
            _reservationRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reservations);

            // Act
            var result = await _reservationService.GetReservations();

            // Assert
            result.Should().BeEquivalentTo(reservations);
        }

        [Fact]
        public async Task TryCreateReservation_WithAvailableTableAndNoConflict_ShouldCreate()
        {
            // Arrange
            var date = DateTime.Today;
            var timeSlot = TimeSlotEnum.SLOT_2330;
            var table = new Table(1, TableCapacityEnum.Two);
            var reservation = new Reservation(date, timeSlot, table.Id, 1);

            _reservationRepositoryMock.Setup(r => r.GetFirstAvailableTable(date, timeSlot, TableCapacityEnum.Two)).ReturnsAsync(table);
            _reservationRepositoryMock.Setup(r => r.ClientHasSameReservation(1, date, timeSlot)).ReturnsAsync(false);
            _reservationRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Reservation>()))
                .Callback<Reservation>(res => reservation = res)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _reservationService.TryCreateReservation(date, timeSlot, TableCapacityEnum.Two, 1);

            // Assert
            result.Should().NotBeNull();
            result.TableId.Should().Be(table.Id);
            result.ClientId.Should().Be(1);
        }

        [Fact]
        public async Task TryCreateReservation_WhenNoTableAvailable_ShouldReturnNull()
        {
            // Arrange
            var date = DateTime.Today;
            var timeSlot = TimeSlotEnum.SLOT_2330;
            _reservationRepositoryMock.Setup(r => r.GetFirstAvailableTable(date, timeSlot, TableCapacityEnum.Two)).ReturnsAsync((Table)null);

            // Act
            var result = await _reservationService.TryCreateReservation(date, timeSlot, TableCapacityEnum.Two, 1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task TryCreateReservation_WhenClientAlreadyHasReservation_ShouldThrow()
        {
            // Arrange
            var date = DateTime.Today;
            var timeSlot = TimeSlotEnum.SLOT_2330;
            var table = new Table(1, TableCapacityEnum.Two);

            _reservationRepositoryMock.Setup(r => r.GetFirstAvailableTable(date, timeSlot, TableCapacityEnum.Two)).ReturnsAsync(table);
            _reservationRepositoryMock.Setup(r => r.ClientHasSameReservation(1, date, timeSlot)).ReturnsAsync(true);

            // Act
            var act = async () => await _reservationService.TryCreateReservation(date, timeSlot, TableCapacityEnum.Two, 1);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Ésta reserva ya existe.");
        }

        [Fact]
        public async Task CancelReservation_WhenWaitingListIsEmpty_ShouldOnlyDeleteReservation()
        {
            // Arrange
            var date = DateTime.Today;
            var timeSlot = TimeSlotEnum.SLOT_2330;
            var reservation = new Reservation(date, timeSlot, 1, 1);

            _reservationRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(reservation);
            _reservationRepositoryMock.Setup(r => r.DeleteAsync(reservation)).Returns(Task.CompletedTask);
            _waitingListServiceMock.Setup(w => w.GetPrioritizedWaitingListEntry(date, timeSlot, TableCapacityEnum.Two)).ReturnsAsync((WaitingListEntry)null);

            // Act
            var result = await _reservationService.CancelReservation(1);

            // Assert
            result.Should().Be(reservation);
            _reservationRepositoryMock.Verify(r => r.DeleteAsync(reservation), Times.Once);
        }

        [Fact]
        public async Task CancelReservation_WhenWaitingListHasClient_ShouldReassignTable()
        {
            // Arrange
            var date = DateTime.Today;
            var timeSlot = TimeSlotEnum.SLOT_2330; // Aseguramos que este time slot esté configurado correctamente
            var originalReservation = new Reservation(date, timeSlot, 1, 1); // Reserva original

            // Configuración del cliente que está en la lista de espera
            var waitingClient = new Client(2, "Nuevo", "Cliente", CategoryTypeEnum.Gold);
            var waitingEntry = new WaitingListEntry(waitingClient, TableCapacityEnum.Two, date, timeSlot);

            // Mock de la repositorio de reservas
            _reservationRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(originalReservation);
            _reservationRepositoryMock.Setup(r => r.DeleteAsync(originalReservation)).Returns(Task.CompletedTask);

            // Mock del servicio de lista de espera
            _waitingListServiceMock.Setup(w => w.GetPrioritizedWaitingListEntry(date, timeSlot, TableCapacityEnum.Two))
                .ReturnsAsync(waitingEntry);  // Aquí estamos devolviendo la entrada correcta de la lista de espera

            // Mock para agregar una nueva reserva
            _reservationRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

            // Mock para eliminar la entrada de la lista de espera
            _waitingListServiceMock.Setup(w => w.RemoveWaitingListEntry(waitingEntry)).Returns(Task.CompletedTask);

            // Act
            var result = await _reservationService.CancelReservation(1);

            // Assert
            result.Should().Be(originalReservation); // Verificamos que la reserva cancelada es la correcta
            _reservationRepositoryMock.Verify(r => r.DeleteAsync(originalReservation), Times.Once); // Verificamos que la reserva original fue eliminada
            _reservationRepositoryMock.Verify(r => r.AddAsync(It.Is<Reservation>(r =>
                r.ClientId == waitingClient.Id &&
                r.TableId == originalReservation.TableId &&
                r.ReservationDate == date)), Times.Once); // Verificamos que se añadió la nueva reserva con el cliente de la lista de espera
            _waitingListServiceMock.Verify(w => w.RemoveWaitingListEntry(waitingEntry), Times.Once); // Verificamos que la entrada fue eliminada de la lista de espera
        }


        [Fact]
        public void ValidateClientCouldReserve_WhenNotAllowed_ShouldThrow()
        {
            // Arrange
            var client = new Client(1, "Test", "User", CategoryTypeEnum.Classic);
            var futureDate = DateTime.Today.AddDays(60);
            var timeSlot = TimeSlotEnum.SLOT_2100;

            // Act
            Action act = () => _reservationService.ValidateClientCouldReserve(client, futureDate, timeSlot);

            // Assert
            act.Should().Throw<Exception>().WithMessage("El cliente no puede reservar con tanta anticipación.");
        }

        [Fact]
        public async Task TryCreateReservation_WhenNoTableAvailableDueToOverlap_ShouldReturnNull()
        {
            // Arrange
            var date = DateTime.Today;
            var timeSlot = TimeSlotEnum.SLOT_2000;

            // Simulamos que no hay mesa disponible porque todas están ocupadas por solapamiento
            _reservationRepositoryMock
                .Setup(r => r.GetFirstAvailableTable(date, timeSlot, TableCapacityEnum.Four))
                .ReturnsAsync((Table)null);

            // Act
            var result = await _reservationService.TryCreateReservation(date, timeSlot, TableCapacityEnum.Four, 1);

            // Assert
            result.Should().BeNull();
        }
    }
}