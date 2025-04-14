using HipodromoReservas.Domain.Enums;

namespace HipodromoReservas.Domain.Constans
{
    public static class TimeSlotConstants
    {
        public const int ReservationDurationInSlots = 3;
        public const int MinSlot = (int)TimeSlotEnum.SLOT_2000;
        public const int MaxSlot = (int)TimeSlotEnum.SLOT_2330;
    }
}