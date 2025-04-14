using HipodromoReservas.Domain.Constans;
using HipodromoReservas.Domain.Entities;
using HipodromoReservas.Domain.Enums;
using HipodromoReservas.Domain.Interfaces.IRepository;
using HipodromoReservas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HipodromoReservas.Infrastructure.Repositories
{
    public class WaitingListRepository : IWaitingListRepository
    {
        private readonly HipodromoReservaContext _context;

        public WaitingListRepository(HipodromoReservaContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WaitingListEntry entry)
        {
            await _context.ClientWaitingList.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(WaitingListEntry entry)
        {
            _context.ClientWaitingList.Remove(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WaitingListEntry>> GetAllAsync()
        {
            return await _context.ClientWaitingList
                .OrderByDescending(r => r.ReservationDate)
                .ThenByDescending(r => r.ClientCategory)
                .ThenBy(w => w.RequestedAt)
                .ToListAsync();
        }

        public async Task<WaitingListEntry?> GetByIdAsync(int id)
        {
            return await _context.ClientWaitingList
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasSimilarActiveEntry(int clientId, TableCapacityEnum tableCapacity, DateTime reservationDate, TimeSlotEnum timeSlot)
        {
            return await _context.ClientWaitingList
                .AnyAsync(r =>
                    r.ClientId == clientId &&
                    r.TableCapacity == tableCapacity &&
                    r.ReservationDate == reservationDate &&
                    r.TimeSlot == timeSlot
                );
        }

        public async Task<WaitingListEntry?> GetPrioritizedWaitingListEntry(DateTime reservationDate, TimeSlotEnum timeSlot, TableCapacityEnum tableCapacity)
        {
            const int reservationDuration = TimeSlotConstants.ReservationDurationInSlots;

            List<WaitingListEntry> candidates = await this.GetWaitingListCandidates(reservationDate, tableCapacity);

            if (!candidates.Any())
                return null;

            // Obtenemos todas las mesas de esa capacidad
            var allTables = TableConstants.Tables
                .Where(t => t.Capacity == tableCapacity)
                .Select(t => t.Id)
                .ToList();

            // Calculamos el rango mínimo y máximo de slots de todos los candidatos
            var minSlot = candidates.Min(c => (int)c.TimeSlot);
            var maxSlot = candidates.Max(c => (int)c.TimeSlot) + reservationDuration - 1;

            // Traemos solo las reservas que podrían solaparse con algún candidato y en mesas válidas
            var reservations = await _context.Reservations
                .Where(r => r.ReservationDate == reservationDate &&
                            allTables.Contains(r.TableId) &&
                            (int)r.TimeSlot <= maxSlot &&
                            (int)r.TimeSlot + (reservationDuration - 1) >= minSlot)
                .Select(r => new
                {
                    r.TableId,
                    Start = (int)r.TimeSlot,
                    End = (int)r.TimeSlot + (reservationDuration - 1)
                })
                .ToListAsync();

            // Se agrupan las reservas por mesa para acceso rápido
            var reservationsByTable = reservations
                .GroupBy(r => r.TableId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var candidate in candidates)
            {
                var candidateStart = (int)candidate.TimeSlot;
                var candidateEnd = candidateStart + (reservationDuration - 1);

                // Verificamos si alguna mesa está completamente libre en ese rango para este candidato
                var hasAvailableTable = allTables.Any(tableId =>
                {
                    if (!reservationsByTable.TryGetValue(tableId, out var tableReservations))
                        return true; // Mesa sin reservas

                    return !tableReservations.Any(r =>
                        candidateStart <= r.End && r.Start <= candidateEnd
                    );
                });

                if (hasAvailableTable)
                {
                    return candidate;
                }
            }

            return null;
        }

        private async Task<List<WaitingListEntry>> GetWaitingListCandidates(DateTime reservationDate, TableCapacityEnum tableCapacity)
        { 
            return await _context.ClientWaitingList
                .Where(entry => entry.ReservationDate.Date == reservationDate.Date &&
                                entry.TableCapacity == tableCapacity &&
                                entry.ClientId > 0)
                .OrderByDescending(entry => entry.ClientCategory)
                .ThenBy(entry => entry.RequestedAt)
                .ToListAsync();
        }
    }
}