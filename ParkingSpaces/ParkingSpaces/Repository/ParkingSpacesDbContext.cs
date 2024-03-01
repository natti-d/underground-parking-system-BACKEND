using Microsoft.EntityFrameworkCore;
using ParkingSpaces.Models.DB;

namespace ParkingSpaces
{
    public class ParkingSpacesDbContext : DbContext
    {

        public ParkingSpacesDbContext() { }
        public ParkingSpacesDbContext(DbContextOptions<ParkingSpacesDbContext> options)
            : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<ParkSpace> ParkSpaces { get; set; }
        public DbSet<Booking> Bookings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkSpace>().HasData(
                 new ParkSpace() { Id = 1, Name = "A1"},
                 new ParkSpace() { Id = 2, Name = "A2" },
                 new ParkSpace() { Id = 3, Name = "A3" },
                 new ParkSpace() { Id = 4, Name = "A4" },
                 new ParkSpace() { Id = 5, Name = "A5" },
                 new ParkSpace() { Id = 6, Name = "A6" },
                 new ParkSpace() { Id = 7, Name = "A7" },
                 new ParkSpace() { Id = 8, Name = "A8" },
                 new ParkSpace() { Id = 9, Name = "A9" },
                 new ParkSpace() { Id = 10, Name = "B1" },
                 new ParkSpace() { Id = 11, Name = "B2" },
                 new ParkSpace() { Id = 12, Name = "B3" },
                 new ParkSpace() { Id = 13, Name = "B4" },
                 new ParkSpace() { Id = 14, Name = "B5" },
                 new ParkSpace() { Id = 15, Name = "B6" },
                 new ParkSpace() { Id = 16, Name = "B7" },
                 new ParkSpace() { Id = 17, Name = "disabled" }
             );
        }
    }
}