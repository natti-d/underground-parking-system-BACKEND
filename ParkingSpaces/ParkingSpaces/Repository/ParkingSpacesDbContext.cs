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
            //modelBuilder.Entity<ParkSpace>().HasNoKey();

            // one to one
            // one to many
            //modelBuilder.Entity<Booking>()
            //   .HasOne(b => b.ParkSpace)       // Booking has one ParkSpace
            //   .WithOne(b => b.Booking)         // ParkSpace can be in many bookings
            //   .HasForeignKey<Booking>(b => b.ParkSpaceId) // Use BookingId as foreign key in ParkSpace
            //   .IsRequired();
        }
    }
}