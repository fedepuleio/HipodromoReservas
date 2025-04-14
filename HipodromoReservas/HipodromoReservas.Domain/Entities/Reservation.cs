using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; private set; }
        public DateTime ReservationDate { get; private set; }
        public TimeSlotEnum TimeSlot { get; private set; }
        public int TableId { get; private set; }
        public int ClientId { get; private set; }

        public Reservation() { }
        public Reservation(DateTime reservation, TimeSlotEnum timeSlot, int tableId, int clientId)
        {
            this.ReservationDate = reservation;
            this.TableId = tableId;
            this.ClientId = clientId;
            this.TimeSlot = timeSlot;
        }
    }
}