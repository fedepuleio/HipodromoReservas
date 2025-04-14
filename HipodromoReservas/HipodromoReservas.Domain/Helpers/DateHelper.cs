using HipodromoReservas.Domain.Enums;
using System;

namespace HipodromoReservas.Application.Utils
{
    public static class DateHelper
    {
        /// <summary>
        /// Valida que la fecha no sea anterior al día actual y sea una fecha válida.
        /// </summary>
        /// <param name="date">La fecha a validar.</param>
        public static void ValidateDate(DateTime date)
        {
            if (date.ToUniversalTime().Date < DateTime.UtcNow.Date)
                throw new ArgumentOutOfRangeException("La fecha no puede ser anterior a hoy");

            try
            {
                var validDate = new DateTime(date.Year, date.Month, date.Day);
            }
            catch
            {
                throw new ArgumentOutOfRangeException("La fecha no es válida.");
            }
        }

        /// <summary>
        /// Obtiene la hora de inicio para un timeSlot específico.
        /// </summary>
        /// <param name="timeSlot">El timeSlot del que se quiere obtener la hora de inicio.</param>
        /// <returns>Un <see cref="DateTime"/> que representa la hora de inicio del timeSlot.</returns>
        public static DateTime GetTimeSlotStart(TimeSlotEnum timeSlot)
        {
            switch (timeSlot)
            {
                case TimeSlotEnum.SLOT_2000:
                    return new DateTime(1, 1, 1, 20, 0, 0); // 20:00
                case TimeSlotEnum.SLOT_2030:
                    return new DateTime(1, 1, 1, 20, 30, 0); // 20:30
                case TimeSlotEnum.SLOT_2100:
                    return new DateTime(1, 1, 1, 21, 0, 0); // 21:00
                case TimeSlotEnum.SLOT_2130:
                    return new DateTime(1, 1, 1, 21, 30, 0); // 21:30
                case TimeSlotEnum.SLOT_2200:
                    return new DateTime(1, 1, 1, 22, 0, 0); // 22:00
                case TimeSlotEnum.SLOT_2230:
                    return new DateTime(1, 1, 1, 22, 30, 0); // 22:30
                case TimeSlotEnum.SLOT_2300:
                    return new DateTime(1, 1, 1, 23, 0, 0); // 23:00
                case TimeSlotEnum.SLOT_2330:
                    return new DateTime(1, 1, 1, 23, 30, 0); // 23:30
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeSlot), "Franja horaria inválida");
            }
        }

        /// <summary>
        /// Construye la fecha completa de reserva combinando la fecha proporcionada y la hora correspondiente al timeSlot.
        /// </summary>
        /// <param name="reservationDate">La fecha de la reserva (sin la hora).</param>
        /// <param name="timeSlot">El timeSlot que se desea asignar a la reserva.</param>
        /// <returns>Un <see cref="DateTime"/> que representa la fecha y hora completa de la reserva.</returns>
        public static DateTime BuildReservationDate(DateTime reservationDate, TimeSlotEnum timeSlot)
        {
            // Primero validamos la fecha
            ValidateDate(reservationDate);

            // Luego obtenemos la hora del timeSlot
            var timeSlotStart = GetTimeSlotStart(timeSlot);

            // Construimos la fecha final con la fecha proporcionada y la hora del timeSlot
            var reservationDateTime = new DateTime(reservationDate.Year, reservationDate.Month, reservationDate.Day, timeSlotStart.Hour, timeSlotStart.Minute, 0);

            return reservationDateTime;
        }
    }
}