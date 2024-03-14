using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Repository.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ParkingSpaces.Models.Response;

namespace ParkingSpaces.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly Lazy<IParkSpaceService> _parkSpaceService;

        public BookingService(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            Lazy<IParkSpaceService> parkSpaceService)
        {
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _parkSpaceService = parkSpaceService;
        }

        public virtual async Task Create(BookingRequest request, int userId)
        {
            User user = await _userRepository.FindById(userId);

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
            if (request.ParkSpaceId < 1 || request.ParkSpaceId > 17)
            {
                throw new Exception("Incorrect park space!");
            }

            // validate the startTime is valid time
            DateTime curr = request.StartTime.ToUniversalTime();
            if (request.StartTime.ToUniversalTime() < DateTime.UtcNow)
            {
                throw new Exception("Incorrect start date!");
            }
                
            Expression<Func<Booking, bool>> expression = booking => 
                    ((booking.StartTime <= request.StartTime && booking.EndTime >= request.StartTime) // if StartTime is in the range of already inserted
                    || (booking.StartTime <= request.StartTime + request.Duration && booking.EndTime >= request.StartTime + request.Duration)); // if StartTime + duration is in the range of already inserted

            Booking presentedBooking = await _bookingRepository
                .FindByCriteria(expression)
                .FirstOrDefaultAsync(b => b.ParkSpace.Id == request.ParkSpaceId);

            if (presentedBooking != null)
            {
                throw new Exception("Already reserved");
            }

            // prevent one user to reserve all available park spaces
            Booking reservedBooking = await _bookingRepository
                .FindByCriteria(expression)
                .FirstOrDefaultAsync(b => b.UserId == userId);
            
            if (reservedBooking != null)
            {
                throw new Exception("Oops! It seems you're already booked for a park space. Only one reservation at a time, please.");
            }

            Booking newBooking = new Booking();

            newBooking.ParkSpaceId = request.ParkSpaceId;
            newBooking.Duration = request.Duration;
            newBooking.StartTime = request.StartTime;
            newBooking.EndTime = request.StartTime + request.Duration;
            newBooking.UserId = user.Id;

            await _bookingRepository.Create(newBooking);
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

            if (user == null)
            {
                throw new Exception("Incorrect credentials!");
            }

            Expression<Func<Booking, bool>> findAvailableExpression = booking => booking.UserId == user.Id
                && ((booking.StartTime <= DateTime.UtcNow && booking.EndTime >= DateTime.UtcNow)
                    || booking.StartTime >= DateTime.UtcNow);

            IQueryable<Booking> activeBookings = _bookingRepository
                .FindByCriteria(findAvailableExpression)
                .Include(b => b.ParkSpace);

            var activeBookingsResponse = activeBookings.Select(booking => new BookingResponse
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                ParkSpaceName = booking.ParkSpace.Name,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            });

            return activeBookingsResponse;
        }

        public virtual async Task<BookingResponse> GetById(int bookingId)
        {
            Booking booking = await _bookingRepository
                .FindById(bookingId);

            if (booking == null)
            {
                throw new Exception("Booking not presented!");
            }

            ParkSpaceResponse parkSpace = await _parkSpaceService.Value
                .GetById(booking.ParkSpaceId);

            return new BookingResponse()
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                ParkSpaceName = parkSpace.Name,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            };
        }

        public virtual async Task<IQueryable<BookingResponse>> GetAllActive()
        {
            Expression<Func<Booking, bool>> findAvailableExpression = booking =>
                ((booking.StartTime <= DateTime.UtcNow && booking.EndTime >= DateTime.UtcNow)
                || booking.StartTime >= DateTime.UtcNow);

            var activeBookings = _bookingRepository
                .FindByCriteria(findAvailableExpression)
                .Include(b => b.ParkSpace);

            var activeBookingsResponse = activeBookings.Select(booking => new BookingResponse
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                ParkSpaceName = booking.ParkSpace.Name,
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
                .FindByCriteria(expression)
                .Include(b => b.ParkSpace);

            var activeBookingsResponse = activeBookings.Select(booking => new BookingResponse
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                ParkSpaceName = booking.ParkSpace.Name,
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
                .FindByCriteria(expression)
                .Include(b => b.ParkSpace);

            var available = availableBookingParkSpaces.Select(booking => new BookingResponse
            {
                BookingId = booking.Id,
                ParkSpaceId = booking.ParkSpaceId,
                ParkSpaceName = booking.ParkSpace.Name,
                Duration = booking.Duration,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            });

            return available;
        }

        // background process
        public virtual async Task DeleteOldBookings(int days)
        {
            Expression<Func<Booking, bool>> expression = booking =>
                booking.EndTime.AddDays(days) <= DateTime.UtcNow;

            IQueryable<Booking> bookings = _bookingRepository
               .FindByCriteria(expression);

            await _bookingRepository.DeleteRange(bookings);
        }
    }
}
