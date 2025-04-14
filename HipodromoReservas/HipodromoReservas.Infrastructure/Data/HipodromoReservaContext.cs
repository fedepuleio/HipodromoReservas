using Microsoft.EntityFrameworkCore;
using HipodromoReservas.Domain.Entities;

namespace HipodromoReservas.Infrastructure.Data
{
    public class HipodromoReservaContext : DbContext
    {
        public HipodromoReservaContext(DbContextOptions<HipodromoReservaContext> options)
            : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<WaitingListEntry> ClientWaitingList { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>().HasKey(r => r.Id);
            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Table>().HasKey(t => t.Id);
            modelBuilder.Entity<WaitingListEntry>().HasKey(t => t.Id);


            modelBuilder.Entity<Reservation>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<WaitingListEntry>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}