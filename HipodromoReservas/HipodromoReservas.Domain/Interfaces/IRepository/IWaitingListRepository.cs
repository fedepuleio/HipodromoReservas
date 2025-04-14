using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Interfaces.IRepository
{
    public interface IWaitingListRepository : IRepository<WaitingListEntry>
    {
        Task<WaitingListEntry?> GetPrioritizedWaitingListEntry(DateTime reservationDate, TimeSlotEnum timeSlot, TableCapacityEnum tableCapacity);
        Task<bool> HasSimilarActiveEntry(int clientId, TableCapacityEnum tableCapacity, DateTime reservationDate, TimeSlotEnum timeSlot);
    }
}