using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Entities
{
    public class WaitingListEntry
    {
        public int Id { get; private set; }
        public int ClientId { get; private set; }
        public int ClientCategory { get; private set; }
        public DateTime ReservationDate { get; private set; }
        public TimeSlotEnum TimeSlot { get; private set; }
        public DateTime RequestedAt { get; private set; }
        public TableCapacityEnum TableCapacity { get; private set; }

        public WaitingListEntry() { }

        public WaitingListEntry(Client client, TableCapacityEnum tableCapacity, DateTime reservationDate, TimeSlotEnum timeSlot)
        {
            ClientId = client.Id;
            ClientCategory = (int)client.Category;
            TableCapacity = tableCapacity;
            ReservationDate = reservationDate;
            RequestedAt = DateTime.Now;
            TimeSlot = timeSlot;
        }
    }
}