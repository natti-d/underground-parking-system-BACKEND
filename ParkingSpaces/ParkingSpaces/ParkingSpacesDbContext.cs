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
    }
}