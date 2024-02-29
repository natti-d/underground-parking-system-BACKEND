using ParkingSpaces.Models.DB;

namespace ParkingSpaces.Repository.Repository_Interfaces
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task DeleteRange(IQueryable<Booking> entities);
    }
}
