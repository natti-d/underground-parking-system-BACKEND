
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ParkingSpaces.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IAuthService authService,
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
        }

        public virtual async Task CreateBooking(BookingCreateBookingRequest bookingRequest, string username)
        {
            Expression<Func<User, bool>> findUserExpression = user => user.Username == username;

            User currUser = _userRepository.FindByCriteria(findUserExpression)
                .FirstOrDefault();

            if (currUser == null)
            {
                throw new Exception("Incorrect credentials!");
            }

            // validate the parkspace id (static data)
            if (bookingRequest.ParkSpaceId < 1 || bookingRequest.ParkSpaceId > 16)
            {
                throw new Exception("Incorrect park space!");
            }

            // validate the startTime is valid time
            if(bookingRequest.StartTime < DateTime.UtcNow)
            {
                throw new Exception("Incorrect start date!");
            }

            Expression<Func<Booking, bool>> expression = booking => booking.ParkSpace.Id == bookingRequest.ParkSpaceId
                && ((booking.StartTime <= bookingRequest.StartTime && booking.EndTime >= bookingRequest.StartTime) // if StartTime is in the range of already inserted
                || (booking.StartTime <= bookingRequest.StartTime + bookingRequest.Duration && booking.StartTime >= bookingRequest.StartTime + bookingRequest.Duration)); // if StartTime + duration is in the range of already inserted

            var presentedBooking = _bookingRepository.FindByCriteria(expression)
                .FirstOrDefault();

            if (presentedBooking != null)
            {
                throw new Exception("Already reserved");
            }
            else
            {
                Booking newBooking = new Booking();

                newBooking.ParkSpaceId = bookingRequest.ParkSpaceId;
                newBooking.Duration = bookingRequest.Duration;
                newBooking.StartTime = bookingRequest.StartTime;
                newBooking.EndTime = bookingRequest.StartTime + bookingRequest.Duration;
                newBooking.UserId = currUser.Id;

                await _bookingRepository.Create(newBooking);
            }
        }

        public Task DeleteBooking(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetAllBookings()
        {
            throw new NotImplementedException();
        }

        public Task UpdateBooking(BookingCreateBookingRequest booking, int id)
        {
            throw new NotImplementedException();
        }
    }
}
