namespace HipodromoReservas.Application.Dtos
{
    public class CreateReservationResponseDto
    {
        public bool WaitingList {  get; set; }
        public int? TableId { get; set; }
    }
}