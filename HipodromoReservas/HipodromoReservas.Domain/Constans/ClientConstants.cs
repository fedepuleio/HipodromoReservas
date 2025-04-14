using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Constans
{
    public static class ClientConstants
    {
        public static readonly List<Client> Clients =
        [
            new Client(1, "Pamela", "Villar", CategoryTypeEnum.Classic),
            new Client(2, "Narda", "Lepes", CategoryTypeEnum.Gold),
            new Client(3, "Maru", "Botana", CategoryTypeEnum.Platinum),
            new Client(4, "Dolli", "Irigoyen", CategoryTypeEnum.Diamond),
            new Client(5, "Donato", "De Santis", CategoryTypeEnum.Gold),
            new Client(6, "Germán", "Martitegui", CategoryTypeEnum.Diamond),
            new Client(7, "Christophe", "Krywonis", CategoryTypeEnum.Platinum),
            new Client(8, "Paula", "Chaves", CategoryTypeEnum.Classic),
            new Client(9, "Juana", "Viale", CategoryTypeEnum.Gold),
            new Client(10, "Santiago", "Del Moro", CategoryTypeEnum.Platinum),
            new Client(11, "Florencia", "Peña", CategoryTypeEnum.Classic),
            new Client(12, "Rodrigo", "Guirao", CategoryTypeEnum.Classic),
            new Client(13, "Lizy", "Tagliani", CategoryTypeEnum.Gold),
            new Client(14, "Cande", "Tinelli", CategoryTypeEnum.Classic),
            new Client(15, "Carmen", "Barbieri", CategoryTypeEnum.Platinum),
            new Client(16, "Mirtha", "Legrand", CategoryTypeEnum.Diamond),
            new Client(17, "Nico", "Occhiato", CategoryTypeEnum.Classic),
            new Client(18, "Valeria", "Mazza", CategoryTypeEnum.Gold),
            new Client(19, "Pablo", "Granados", CategoryTypeEnum.Platinum),
            new Client(20, "Guido", "Kaczka", CategoryTypeEnum.Diamond)
        ];

        public static Client GetClientById(int clientId)
        {
            var client = Clients.FirstOrDefault(c => c.Id == clientId);
            if (client == null)
                throw new Exception("Cliente Inexistente.");
            return client;
        }
    }
}