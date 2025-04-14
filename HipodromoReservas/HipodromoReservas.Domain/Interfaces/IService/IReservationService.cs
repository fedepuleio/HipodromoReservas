using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Interfaces.IService
{
    public interface IReservationService
    {
        Task<Reservation?> TryCreateReservation(DateTime reservationDate, TimeSlotEnum timeSlot, TableCapacityEnum tableCapacity, int clientId);
        Task<Reservation> CancelReservation(int reservationId);
        Task<List<Reservation>> GetReservations();
        void ValidateClientCouldReserve(Client client, DateTime reservationDate, TimeSlotEnum timeSlot);
    }
}