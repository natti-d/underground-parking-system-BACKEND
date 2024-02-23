
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.NewFolder;
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

        public virtual async Task CreateBooking(BookingCreateBookingRequest request, string username)
        {
            Expression<Func<User, bool>> findUserExpression = user => user.Username == username;

            User currUser = _userRepository.FindByCriteria(findUserExpression)
                .FirstOrDefault();

            // this will never be hit
            if (currUser == null)
            {
                throw new Exception("Incorrect credentials!");
            }

            if (request.Duration.TotalHours > 8
                || request.Duration.TotalMilliseconds == 0)
            {
                throw new Exception("Incorrect duration!");
            }

            // validate the parkspace id (static data)
            if (request.ParkSpaceId < 1 || request.ParkSpaceId > 16)
            {
                throw new Exception("Incorrect park space!");
            }

            // validate the startTime is valid time
            if(request.StartTime < DateTime.UtcNow)
            {
                throw new Exception("Incorrect start date!");
            }

            Expression<Func<Booking, bool>> expression = booking => booking.ParkSpace.Id == request.ParkSpaceId
                && ((booking.StartTime <= request.StartTime && booking.EndTime >= request.StartTime) // if StartTime is in the range of already inserted
                    || (booking.StartTime <= request.StartTime + request.Duration && booking.StartTime >= request.StartTime + request.Duration)); // if StartTime + duration is in the range of already inserted

            var presentedBooking = _bookingRepository.FindByCriteria(expression)
                .FirstOrDefault();

            if (presentedBooking != null)
            {
                throw new Exception("Already reserved");
            }
            else
            {
                Booking newBooking = new Booking();

                newBooking.ParkSpaceId = request.ParkSpaceId;
                newBooking.Duration = request.Duration;
                newBooking.StartTime = request.StartTime;
                newBooking.EndTime = request.StartTime + request.Duration;
                newBooking.UserId = currUser.Id;

                await _bookingRepository.Create(newBooking);
            }
        }

        public virtual async Task DeleteBooking(BookingDeleteBookingRequest request)
        {
            Expression<Func<Booking, bool>> findBookingExpression = booking => booking.Id == request.BookingId;
            Booking currBooking = _bookingRepository.FindByCriteria(findBookingExpression)
                .FirstOrDefault();

            if (currBooking != null)
            {
                await _bookingRepository.Delete(currBooking);
            }
            else
            {
                throw new Exception();
            }
        }

        public virtual async Task UpdateBooking(BookingUpdateBookingRequest request)
        {
            if (request.Duration.TotalHours > 8
                || request.Duration.TotalMilliseconds == 0)
            {
                throw new Exception("Incorrect duration!");
            }

            // validate the parkspace id (static data)
            if (request.ParkSpaceId < 1 || request.ParkSpaceId > 16)
            {
                throw new Exception("Incorrect park space!");
            }

            // validate the startTime is valid time
            if (request.StartTime < DateTime.UtcNow)
            {
                throw new Exception("Incorrect start date!");
            }

            Booking currBooking = _bookingRepository.FindById(request.BookingId);

            if (currBooking == null)
            {
                throw new Exception();
            }

            // to test it!
            /*
             [
              {
                "bookingId": 7,
                "parkSpaceId": 2,
                "duration": "00:10:00",
                "startTime": "2024-02-23T20:55:45.806",
                "endTime": "2024-02-23T21:05:45.806"
              },
              {
                "bookingId": 8,
                "parkSpaceId": 2,
                "duration": "00:10:00",
                "startTime": "2024-02-23T20:50:45.806",
                "endTime": "2024-02-23T21:00:45.806"
              }
            ]
             */
            Expression<Func<Booking, bool>> expression = booking => booking.ParkSpace.Id == request.ParkSpaceId
                && ((booking.StartTime <= request.StartTime && booking.EndTime >= request.StartTime) // if StartTime is in the range of already inserted
                    || (booking.StartTime <= request.StartTime + request.Duration && booking.StartTime >= request.StartTime + request.Duration)); // if StartTime + duration is in the range of already inserted

            var presentedBooking = _bookingRepository.FindByCriteria(expression)
                .FirstOrDefault();

            if (presentedBooking != null)
            {
                throw new Exception("Already reserved");
            }
            else
            {
                currBooking.ParkSpaceId = request.ParkSpaceId;
                currBooking.Duration = request.Duration;
                currBooking.StartTime = request.StartTime;
                currBooking.EndTime = request.StartTime + request.Duration;

                await _bookingRepository.Update(currBooking);
            }
        }

        public virtual async Task<IEnumerable<BookingGetActiveBookingsResponse>> GetActiveBookings(string username)
        {
            Expression<Func<User, bool>> findUserExpression = user => user.Username == username;

            User currUser = _userRepository.FindByCriteria(findUserExpression)
                .FirstOrDefault();

            // this will never be hit
            if (currUser == null)
            {
                throw new Exception("Incorrect credentials!");
            }

            Expression<Func<Booking, bool>> findAvailableExpression = booking => booking.UserId == currUser.Id 
                && ((booking.StartTime <= DateTime.UtcNow && booking.EndTime >= DateTime.UtcNow)
                    || booking.StartTime >= DateTime.UtcNow);

            var activeBookings = _bookingRepository.FindByCriteria(findAvailableExpression);

            var activeBookingsResponse = activeBookings.Select(booking => new BookingGetActiveBookingsResponse
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            });

            return activeBookingsResponse;
        }   
    }
}
