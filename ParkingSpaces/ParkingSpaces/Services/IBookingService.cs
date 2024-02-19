using ParkingSpaces.Models.DB;
using ParkingSpaces.RequestObjects;

namespace ParkingSpaces.Services
{
    public interface IBookingService
    {
        // async

        public Task CreateBooking(BookingRequest booking);

        public Task DeleteBooking(int id);

        public Task UpdateBooking(BookingRequest booking, int id);

        public Task<IEnumerable<Booking>> GetAllBookings();
    }
}
