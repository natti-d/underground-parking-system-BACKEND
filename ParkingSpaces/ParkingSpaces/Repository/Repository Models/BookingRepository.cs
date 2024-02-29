using ParkingSpaces.Models.DB;
using ParkingSpaces.Repository.Repository_Interfaces;

namespace ParkingSpaces.Repository.Repository_Models
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        // NOTE: DI plays with the instances
        public BookingRepository(ParkingSpacesDbContext repository_Context)
            : base(repository_Context)
        {
        }

        public async Task DeleteRange(IQueryable<Booking> entities)
        {
            RepositoryContext.RemoveRange(entities);
            await RepositoryContext.SaveChangesAsync();
        }
    }
}
