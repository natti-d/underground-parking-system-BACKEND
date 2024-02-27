using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Response;

namespace ParkingSpaces.Services
{
    public interface IBookingService
    {
        public Task Create(BookingCreate request, int userId);

        public Task Delete(BookingDelete request);

        public Task Update(BookingUpdate booking);

        public Task<BookingGetAllActive> GetById(int bookingId);

        public Task<IEnumerable<BookingGetAllActive>> GetActiveForUser(int userId);

        public Task<IQueryable<BookingGetAllActive>> GetAllActive();
        public Task<IQueryable<BookingGetAllActive>> GetActiveForNow();

        public Task<IQueryable<BookingGetAvailable>> GetAvailableByFilter(ParkSpaceGetAvailableFilter request);
    }
}
