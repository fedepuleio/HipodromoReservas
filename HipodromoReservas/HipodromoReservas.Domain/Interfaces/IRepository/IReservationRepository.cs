using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Interfaces.IRepository
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Table?> GetFirstAvailableTable(DateTime reservationDate, TimeSlotEnum timeSlot, TableCapacityEnum tableCapacity);
        Task<bool> ClientHasSameReservation(int clientId, DateTime reservationDate, TimeSlotEnum timeSlot);
    }
}