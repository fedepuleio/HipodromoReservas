using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Application.Services
{
    public interface IWaitingListService
    {
        Task<WaitingListEntry> CreateEntry(Client client, TableCapacityEnum tableCapacity, DateTime reservationDate, TimeSlotEnum timeSlot);
        Task RemoveWaitingListEntry(WaitingListEntry entry);
        Task<List<WaitingListEntry>> GetWaitingList();
        Task<WaitingListEntry?> GetPrioritizedWaitingListEntry(DateTime reservationDate, TimeSlotEnum timeSlot, TableCapacityEnum tableCapacity);
    }
}