namespace HipodromoReservas.Tests.Services
{
    public class WaitingListServiceTests
    {
        private readonly Mock<IWaitingListRepository> _waitingListRepositoryMock;
        private readonly WaitingListService _waitingListService;

        public WaitingListServiceTests()
        {
            _waitingListRepositoryMock = new Mock<IWaitingListRepository>();
            _waitingListService = new WaitingListService(_waitingListRepositoryMock.Object);
        }

        [Fact]
        public async Task GetWaitingList_ShouldReturnAllEntries()
        {
            // Arrange
            var client1 = new Client(1, "John", "Doe", CategoryTypeEnum.Gold);
            var client2 = new Client(2, "Jane", "Smith", CategoryTypeEnum.Classic);
            var timeslot = TimeSlotEnum.SLOT_2100;

            var list = new List<WaitingListEntry> {
                new WaitingListEntry(client1, TableCapacityEnum.Two, DateTime.Today, timeslot),
                new WaitingListEntry(client2, TableCapacityEnum.Four, DateTime.Today, timeslot)
            };

            _waitingListRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            // Act
            var result = await _waitingListService.GetWaitingList();

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task CreateEntry_WhenClientNotInList_ShouldCreateEntry()
        {
            // Arrange
            var client = new Client(1, "John", "Doe", CategoryTypeEnum.Gold);
            var date = DateTime.Today;
            var capacity = TableCapacityEnum.Two;
            var timeSlot = TimeSlotEnum.SLOT_2100;

            _waitingListRepositoryMock
                .Setup(r => r.HasSimilarActiveEntry(client.Id, capacity, date, timeSlot))
                .ReturnsAsync(false);

            WaitingListEntry? capturedEntry = null;
            _waitingListRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<WaitingListEntry>()))
                .Callback<WaitingListEntry>(entry => capturedEntry = entry)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _waitingListService.CreateEntry(client, capacity, date, timeSlot);

            // Assert
            capturedEntry.Should().NotBeNull();
            capturedEntry.ClientId.Should().Be(client.Id);
            capturedEntry.TableCapacity.Should().Be(capacity);
            capturedEntry.ReservationDate.Should().Be(date);
            capturedEntry.TimeSlot.Should().Be(timeSlot);

            result.Should().BeEquivalentTo(capturedEntry);
            _waitingListRepositoryMock.Verify(r => r.AddAsync(It.IsAny<WaitingListEntry>()), Times.Once);
        }

        [Fact]
        public async Task CreateEntry_WhenClientAlreadyInList_ShouldThrow()
        {
            // Arrange
            var client = new Client(1, "John", "Doe", CategoryTypeEnum.Gold);
            var date = DateTime.Today;
            var capacity = TableCapacityEnum.Two;
            var timeSlot = TimeSlotEnum.SLOT_2100;

            _waitingListRepositoryMock
                .Setup(r => r.HasSimilarActiveEntry(client.Id, capacity, date, timeSlot))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _waitingListService.CreateEntry(client, capacity, date, timeSlot);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage($"{client.Name} ya estás en la lista de espera!.");
        }

        [Fact]
        public async Task RemoveWaitingListEntry_ShouldCallRepository()
        {
            // Arrange
            var timeSlot = TimeSlotEnum.SLOT_2100;
            var client = new Client(1, "John", "Doe", CategoryTypeEnum.Gold);
            var entry = new WaitingListEntry(client, TableCapacityEnum.Two, DateTime.Today, timeSlot);

            _waitingListRepositoryMock
                .Setup(r => r.DeleteAsync(entry))
                .Returns(Task.CompletedTask);

            // Act
            await _waitingListService.RemoveWaitingListEntry(entry);

            // Assert
            _waitingListRepositoryMock.Verify(r => r.DeleteAsync(entry), Times.Once);
        }

        [Fact]
        public async Task GetPrioritizedWaitingListEntry_ShouldReturnEntry()
        {
            // Arrange
            var date = DateTime.Today;
            var timeSlot = TimeSlotEnum.SLOT_2100;
            var capacity = TableCapacityEnum.Two;
            var expectedEntry = new WaitingListEntry(new Client(1, "John", "Doe", CategoryTypeEnum.Gold), capacity, date, timeSlot);

            _waitingListRepositoryMock
                .Setup(r => r.GetPrioritizedWaitingListEntry(date, timeSlot, capacity))
                .ReturnsAsync(expectedEntry);

            // Act
            var result = await _waitingListService.GetPrioritizedWaitingListEntry(date, timeSlot, capacity);

            // Assert
            result.Should().Be(expectedEntry);
        }
    }
}
