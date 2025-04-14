using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;
using HipodromoReservas.Domain.Interfaces.IRepository;

namespace HipodromoReservas.Application.Services
{
    public class WaitingListService : IWaitingListService
    {
        private readonly IWaitingListRepository _waitingListRepository;

        public WaitingListService(IWaitingListRepository waitingListRepository)
        {
            _waitingListRepository = waitingListRepository;
        }
        public async Task<List<WaitingListEntry>> GetWaitingList()
        {
            List<WaitingListEntry> waitingList = await _waitingListRepository.GetAllAsync();
            return waitingList;
        }

        public async Task<WaitingListEntry> CreateEntry(Client client, TableCapacityEnum tableCapacity, DateTime reservationDate, TimeSlotEnum timeSlot)
        {
            bool isAlredyOnWaitingList = await this.IsAlreadyOnWaitingList(client, tableCapacity, reservationDate, timeSlot);
            if (isAlredyOnWaitingList)
                throw new Exception($"{client.Name} ya estás en la lista de espera!.");

            WaitingListEntry entry = new WaitingListEntry(client, tableCapacity, reservationDate, timeSlot);
            await _waitingListRepository.AddAsync(entry);
            return entry;
        }

        public Task RemoveWaitingListEntry(WaitingListEntry entry)
        {
            return _waitingListRepository.DeleteAsync(entry);
        }

        public async Task<WaitingListEntry?> GetPrioritizedWaitingListEntry(DateTime reservationDate, TimeSlotEnum timeSlot, TableCapacityEnum tableCapacity)
        {
            return await _waitingListRepository.GetPrioritizedWaitingListEntry(reservationDate, timeSlot, tableCapacity);
        }

        private async Task<bool> IsAlreadyOnWaitingList(Client client, TableCapacityEnum tableCapacity, DateTime reservationDate, TimeSlotEnum timeSlot)
        {
            return await _waitingListRepository.HasSimilarActiveEntry(client.Id, tableCapacity, reservationDate, timeSlot);
        }
    }
}