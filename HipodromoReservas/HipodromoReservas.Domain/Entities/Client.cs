using HipodromoReservas.Application.Utils;
using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Entities
{
    public class Client
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public CategoryTypeEnum Category { get; private set; }

        public Client() { }
        public Client(int id, string name, string lastName, CategoryTypeEnum category)
        {
            this.Id = id;
            this.Name = name;
            this.LastName = lastName;
            this.Category = category;
        }

        public bool CouldReserve(DateTime reservationDate, TimeSlotEnum timeSlot)
        {
            if (this.Category == CategoryTypeEnum.Diamond)
                return true;

            DateTime maxPossibleReservationDateTime = DateTime.Now.AddHours((int)this.Category);
            DateTime reservationDateTime = DateHelper.BuildReservationDate(reservationDate, timeSlot);

            return reservationDateTime <= maxPossibleReservationDateTime;
        }
    }
}