using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Repository.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using ParkingSpaces.Models.Response;

namespace ParkingSpaces.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
        }

        public virtual async Task Create(BookingRequest request, int userId)
        {
            User user = await _userRepository.FindById(userId);

            // this will never be hit
            if (user == null)
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
            if (request.StartTime < DateTime.UtcNow)
            {
                throw new Exception("Incorrect start date!");
            }

            Expression<Func<Booking, bool>> expression = booking => booking.ParkSpace.Id == request.ParkSpaceId
                && ((booking.StartTime <= request.StartTime && booking.EndTime >= request.StartTime) // if StartTime is in the range of already inserted
                    || (booking.StartTime <= request.StartTime + request.Duration && booking.EndTime >= request.StartTime + request.Duration)); // if StartTime + duration is in the range of already inserted

            Booking presentedBooking = _bookingRepository
                .FindByCriteria(expression)
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
                newBooking.UserId = user.Id;

                await _bookingRepository.Create(newBooking);
            }
        }

        public virtual async Task Delete(BookingDelete request)
        {
            Expression<Func<Booking, bool>> findBookingExpression = booking => booking.Id == request.BookingId;

            Booking currBooking = _bookingRepository
                .FindByCriteria(findBookingExpression)
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

        public virtual async Task Update(BookingUpdate request)
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

            Booking currBooking = await _bookingRepository.FindById(request.BookingId);

            if (currBooking == null)
            {
                throw new Exception();
            }

            Expression<Func<Booking, bool>> expression = booking => booking.ParkSpace.Id == request.ParkSpaceId
                && ((booking.StartTime <= request.StartTime && booking.EndTime >= request.StartTime) // if StartTime is in the range of already inserted
                    || (booking.StartTime <= request.StartTime + request.Duration && booking.EndTime >= request.StartTime + request.Duration)); // if StartTime + duration is in the range of already inserted

            Booking presentedBooking = _bookingRepository
                .FindByCriteria(expression)
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

        public virtual async Task<IEnumerable<BookingResponse>> GetActiveForUser(int userId)
        {
            User user = await _userRepository.FindById(userId);

            // this will never be hit
            if (user == null)
            {
                throw new Exception("Incorrect credentials!");
            }

            Expression<Func<Booking, bool>> findAvailableExpression = booking => booking.UserId == user.Id
                && ((booking.StartTime <= DateTime.UtcNow && booking.EndTime >= DateTime.UtcNow)
                    || booking.StartTime >= DateTime.UtcNow);

            IQueryable<Booking> activeBookings = _bookingRepository
                .FindByCriteria(findAvailableExpression);

            var activeBookingsResponse = activeBookings.Select(booking => new BookingResponse
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            });

            return activeBookingsResponse;
        }

        public virtual async Task<BookingResponse> GetById(int bookingId)
        {
            Booking booking = await _bookingRepository.FindById(bookingId);

            return new BookingResponse()
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            };
        }

        // return all active bookings
        // optional
        public virtual async Task<IQueryable<BookingResponse>> GetAllActive()
        {
            Expression<Func<Booking, bool>> findAvailableExpression = booking =>
                ((booking.StartTime <= DateTime.UtcNow && booking.EndTime >= DateTime.UtcNow)
                || booking.StartTime >= DateTime.UtcNow);

            var activeBookings = _bookingRepository
                .FindByCriteria(findAvailableExpression);

            var activeBookingsResponse = activeBookings.Select(booking => new BookingResponse
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            });

            return activeBookingsResponse;
        }

        // return all active bookings only for now!
        public virtual async Task<IQueryable<BookingResponse>> GetActiveForNow()
        {
            Expression<Func<Booking, bool>> expression = booking =>
                (booking.StartTime <= DateTime.UtcNow && booking.EndTime >= DateTime.UtcNow);

            IQueryable<Booking> activeBookings = _bookingRepository
                .FindByCriteria(expression);

            var activeBookingsResponse = activeBookings.Select(booking => new BookingResponse
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            });

            return activeBookingsResponse;
        }

        // get available by filter (from, to)
        public virtual async Task<IQueryable<BookingResponse>> GetAvailableByFilter(ParkSpaceGetAvailableByFilter request)
        {
            Expression<Func<Booking, bool>> expression = booking =>
                booking.EndTime <= request.From || booking.StartTime >= request.To;

            IQueryable<Booking> availableBookingParkSpaces = _bookingRepository
                .FindByCriteria(expression);

            var available = availableBookingParkSpaces.Select(booking => new BookingResponse
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            });

            return available;
        }
    }
}
