using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;

namespace ParkingSpaces.Services
{
    public interface IBookingService
    {
        // async

        public Task CreateBooking(BookingCreateBookingRequest booking, string username);

        public Task DeleteBooking(int id);

        public Task UpdateBooking(BookingCreateBookingRequest booking, int id);

        public Task<IEnumerable<Booking>> GetAllBookings();
    }
}
