
using ParkingSpaces.Models.DB;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using ParkingSpaces.RequestObjects;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ParkingSpaces.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public BookingService(
            IBookingRepository bookingRepository,
            IAuthService authService,
            IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
        }

        public virtual async Task CreateBooking(BookingRequest bookingRequest, string username)
        {
            Expression<Func<User, bool>> findUserExpression = user => user.Username == username;

            User currUser = _userRepository.FindByCriteria(findUserExpression);

            if (currUser == null)
            {
                throw new Exception();
            }

            Expression<Func<Booking, bool>> expression = booking => booking.ParkSpace.Id == bookingRequest.ParkSpaceId
                && (booking.StartTime >= bookingRequest.StartTime && booking.EndTime <= bookingRequest.StartTime);

            var precentedBooking = _bookingRepository.FindByCriteria(expression);

            if (precentedBooking != null)
            {
                throw new Exception();
            }
            else
            {
                Booking newBooking = new Booking();

                newBooking.ParkSpaceId = bookingRequest.ParkSpaceId;
                newBooking.Duration = bookingRequest.Duration;
                newBooking.StartTime = bookingRequest.StartTime;
                newBooking.EndTime = bookingRequest.StartTime + bookingRequest.Duration;
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

        public Task UpdateBooking(BookingRequest booking, int id)
        {
            throw new NotImplementedException();
        }
    }
}
