using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.NewFolder;

namespace ParkingSpaces.Services
{
    public interface IBookingService
    {
        // async

        public Task CreateBooking(BookingCreateBookingRequest request, int userId);

        public Task DeleteBooking(BookingDeleteBookingRequest request);

        public Task UpdateBooking(BookingUpdateBookingRequest booking);

        public Task<BookingGetActiveBookingsResponse> GetBookingById(int bookingId);

        public Task<IEnumerable<BookingGetActiveBookingsResponse>> GetActiveBookingsForUser(int userId);

        public Task<IQueryable<BookingGetActiveBookingsResponse>> GetActiveBookings();
        public Task<IQueryable<BookingGetActiveBookingsResponse>> GetActiveBookingsForNow();
    }
}
