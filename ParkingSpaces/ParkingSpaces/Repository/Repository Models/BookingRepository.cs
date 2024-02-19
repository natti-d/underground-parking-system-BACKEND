using ParkingSpaces.Models.DB;

namespace ParkingSpaces.Repository.Repository_Models
{
    public class BookingRepository : BaseRepository<Booking>
    {
        // NOTE: DI plays with the instances
        public BookingRepository(ParkingSpacesDbContext repository_Context)
            : base(repository_Context)
        {
        }
    }
}
