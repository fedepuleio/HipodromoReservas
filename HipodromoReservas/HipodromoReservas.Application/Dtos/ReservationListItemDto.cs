using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Application.Dtos
{
    public class ReservationListItemDto
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public TimeSlotEnum TimeSlot { get; set; }
        public int ClientId { get; set; }
    }
}
