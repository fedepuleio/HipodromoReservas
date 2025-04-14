using HipodromoReservas.Domain.Constans;
using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;
using HipodromoReservas.Domain.Interfaces.IRepository;
using HipodromoReservas.Domain.Interfaces.IService;

namespace HipodromoReservas.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IWaitingListService _waitingListService;

        public ReservationService(
            IReservationRepository reservationRepository,
            IWaitingListService waitingListService
        )
        {
            _reservationRepository = reservationRepository;
            _waitingListService = waitingListService;
        }

        public async Task<List<Reservation>> GetReservations()
        {
            List<Reservation> reservations = await _reservationRepository.GetAllAsync();
            return reservations;
        }

        public async Task<Reservation?> TryCreateReservation(DateTime reservationDate, TimeSlotEnum timeSlot, TableCapacityEnum tableCapacity, int clientId)
        {
            Table? table = await GetFirstAvailableTable(reservationDate, timeSlot, tableCapacity);
            if (table == null)
                return null;

            bool alreadyHasReservation = await this.ClientHasSameReservation(clientId, reservationDate, timeSlot);
            if (alreadyHasReservation)
                throw new Exception($"Ésta reserva ya existe.");

            return await CreateReservation(reservationDate, timeSlot, table.Id, clientId);
        }

        public async Task<Reservation> CancelReservation(int reservationId)
        {
            Reservation? reservation = await _reservationRepository.GetByIdAsync(reservationId);

            if (reservation == null)
                throw new Exception("Reserva inexistente");

            await _reservationRepository.DeleteAsync(reservation);

            // Se reasigna la mesa
            await TryReassignTable(reservation);

            return reservation;
        }

        public void ValidateClientCouldReserve(Client client, DateTime reservationDate, TimeSlotEnum timeSlot)
        {
            if (!client.CouldReserve(reservationDate, timeSlot))
                throw new Exception("El cliente no puede reservar con tanta anticipación.");
        }

        #region Private Methods
        private async Task<Reservation> CreateReservation(DateTime reservationDate, TimeSlotEnum timeSlot, int tableId, int clientId)
        {
            Reservation reservation = new Reservation(reservationDate, timeSlot, tableId, clientId);
            await _reservationRepository.AddAsync(reservation);
            return reservation;
        }

        private async Task TryReassignTable(Reservation deletedReservation)
        {
            Table? table = TableConstants.GetById(deletedReservation.TableId);
            WaitingListEntry? waitingListEntry = await _waitingListService.GetPrioritizedWaitingListEntry(deletedReservation.ReservationDate, deletedReservation.TimeSlot, table.Capacity);

            if (waitingListEntry != null)
            {
                await CreateReservation(deletedReservation.ReservationDate, waitingListEntry.TimeSlot, table.Id, waitingListEntry.ClientId);
                await _waitingListService.RemoveWaitingListEntry(waitingListEntry);
            }
        }

        private Task<bool> ClientHasSameReservation(int clientId, DateTime reservationDate, TimeSlotEnum timeSlot)
        {
            return this._reservationRepository.ClientHasSameReservation(clientId, reservationDate, timeSlot);
        }

        private async Task<Table?> GetFirstAvailableTable(DateTime reservationDate, TimeSlotEnum timeSlot, TableCapacityEnum tableCapacity)
        {
            return await _reservationRepository.GetFirstAvailableTable(reservationDate, timeSlot, tableCapacity);
        }
        #endregion
    }
}