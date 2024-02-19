using ParkingSpaces.Models.DB;
using ParkingSpaces.Repository.Repository_Models;
using ParkingSpaces.RequestObjects;

namespace ParkingSpaces.Services
{
    public class BookingService : IBookingService
    {
        private readonly BookingRepository bookingRepository;

        public virtual async Task CreateBooking(BookingRequest bookingRequest)
        {
            // expression

            Booking booking = new Booking();
            booking.ParkSpaceId = bookingRequest.ParkSpaceId;
            booking.Duration = bookingRequest.Duration;
            booking.StartTime = bookingRequest.StartTime;
            booking.EndTime = bookingRequest.StartTime + booking.Duration; // testing?

            await bookingRepository.Create(booking);
        }

        public Task DeleteBooking(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetAllBookings()
        {
            throw new NotImplementedException();
        }

        public Task UpdateBooking(BookingRequest booking, int id)
        {
            throw new NotImplementedException();
        }
    }
}
