using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Application.Dtos
{
    public class CreateReservationRequestDto
    {
        public int ClientId { get; set; }
        public int TableCapacity { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSlotEnum TimeSlot { get; set; }
    }
}