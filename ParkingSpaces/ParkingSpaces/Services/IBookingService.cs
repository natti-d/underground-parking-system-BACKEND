using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;

namespace ParkingSpaces.Services
{
    public interface IBookingService
    {
        // async

        public Task CreateBooking(BookingCreateBookingRequest request, string username);

        public Task DeleteBooking(BookingDeleteBookingRequest request);

        public Task UpdateBooking(BookingUpdateBookingRequest booking);

        public Task<IEnumerable<BookingGetActiveBookingsResponse>> GetActiveBookings(string username);
    }
}
