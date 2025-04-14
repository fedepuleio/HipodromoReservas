using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public TableCapacityEnum Capacity { get; set; }

        public Table() { }
        public Table(int id, TableCapacityEnum capacity)
        {
            this.Id = id;
            this.Capacity = capacity;
        }
    }
}