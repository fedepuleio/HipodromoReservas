using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Constans
{
    public static class TableConstants
    {
        public static readonly List<Table> Tables = InitializeTables();

        private static List<Table> InitializeTables()
        {
            var tables = new List<Table>();
            int id = 1;

            for (int i = 0; i < 18; i++)
            {
                tables.Add(new Table(id++, TableCapacityEnum.Two));
            }

            for (int i = 0; i < 15; i++)
            {
                tables.Add(new Table(id++, TableCapacityEnum.Four));
            }

            for (int i = 0; i < 7; i++)
            {
                tables.Add(new Table(id++, TableCapacityEnum.Six));
            }

            return tables;
        }

        public static Table GetById(int id)
        {
            var table = Tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
                throw new Exception("Mesa Inexistente.");

            return table;
        }

        public static Table? GetFirstAvailableTable(List<int> occupiedTables, TableCapacityEnum desiredCapacity)
        {
            return Tables
                .Where(t => t.Capacity == desiredCapacity && !occupiedTables.Contains(t.Id))
                .FirstOrDefault();
        }
    }
}