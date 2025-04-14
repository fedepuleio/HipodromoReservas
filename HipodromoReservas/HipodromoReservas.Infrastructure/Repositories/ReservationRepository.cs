using HipodromoReservas.Domain.Constans;
using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;
using HipodromoReservas.Domain.Interfaces.IRepository;
using HipodromoReservas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HipodromoReservas.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HipodromoReservaContext _context;

        public ReservationRepository(HipodromoReservaContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<Table?> GetFirstAvailableTable(DateTime reservationDate, TimeSlotEnum timeSlot, TableCapacityEnum tableCapacity)
        {
            List<int>? occupiedTableIds = await GetOccupiedTableIds(reservationDate, timeSlot);

            var availableTable = TableConstants.Tables
                .Where(t => t.Capacity == tableCapacity && !occupiedTableIds.Contains(t.Id))
                .FirstOrDefault();

            return availableTable;
        }

        public async Task<bool> ClientHasSameReservation(int clientId, DateTime reservationDate, TimeSlotEnum timeSlot)
        {
            return await _context.Reservations
                .AnyAsync(r =>
                    r.ClientId == clientId &&
                    r.ReservationDate == reservationDate &&
                    r.TimeSlot == timeSlot
                );
        }

        private async Task<List<int>> GetOccupiedTableIds(DateTime reservationDate, TimeSlotEnum requestedSlot)
        {
            int requestedStart = (int)requestedSlot;
            int requestedEnd = requestedStart + TimeSlotConstants.ReservationDurationInSlots - 1;

            // Traemos todas las reservas de ese día
            var reservations = await _context.Reservations
                .Where(r => r.ReservationDate == reservationDate)
                .ToListAsync();

            // Recorremos cada reserva existente y vemos si se solapa con el slot pedido
            return reservations
                .Where(r =>
                {
                    int existingStart = (int)r.TimeSlot;
                    int existingEnd = existingStart + TimeSlotConstants.ReservationDurationInSlots - 1;

                    return requestedStart <= existingEnd && existingStart <= requestedEnd;
                })
                .Select(r => r.TableId)
                .Distinct()
                .ToList(); ;
        }
    }
}