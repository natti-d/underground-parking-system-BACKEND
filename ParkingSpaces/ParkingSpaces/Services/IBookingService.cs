using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Response;

namespace ParkingSpaces.Services
{
    public interface IBookingService
    {
        public Task Create(BookingRequest request, int userId);

        public Task Delete(BookingDelete request);

        public Task Update(BookingUpdate booking);

        public Task<BookingResponse> GetById(int bookingId);

        public Task<IEnumerable<BookingResponse>> GetActiveForUser(int userId);

        public Task<IEnumerable<BookingResponse>> GetAll(int userId, int page, int count);
        public Task<IQueryable<BookingResponse>> GetActiveForNow();

        public Task<IQueryable<BookingResponse>> GetAvailableByFilter(ParkSpaceGetAvailableByFilter request);

        public Task DeleteOldBookings(int days);
    }
}
