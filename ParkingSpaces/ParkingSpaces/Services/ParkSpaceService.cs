using Microsoft.EntityFrameworkCore;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Repository.Repository_Interfaces;
using System.Linq.Expressions;

namespace ParkingSpaces.Services
{
    public class ParkSpaceService : IParkSpaceService
    {
        private readonly IParkSpaceRepository _parkSpaceRepository;
        private readonly IBookingService _bookingService;

        public ParkSpaceService(
            IParkSpaceRepository parkSpaceRepository,
            IBookingService bookingService)
        {
            _parkSpaceRepository = parkSpaceRepository;
            _bookingService = bookingService;
        }
        
        // get all available park spaces for now
        public virtual async Task<IEnumerable<ParkSpaceGetAvaildable>> GetAvailable()
        {
            IQueryable<BookingGetAllActive> activeBookings = await _bookingService
                .GetActiveForNow();

            Expression<Func<ParkSpace, bool>> expression = parkSpace => !activeBookings
                .Any(b => b.ParkSpaceId == parkSpace.Id);

            IQueryable<ParkSpace> parkSpaces = _parkSpaceRepository
                .FindByCriteria(expression);

            var availdableParkSpaces = parkSpaces.Select(parkSpace => new ParkSpaceGetAvaildable
            {
                ParkSpaceId = parkSpace.Id,
                Name = parkSpace.Name
            });

            return await availdableParkSpaces
                .ToListAsync();
        }

        // get all available park spaces by filter (from, to)
        public virtual async Task<IEnumerable<ParkSpaceGetAvaildable>> GetAvailableByFilter(ParkSpaceGetAvailableFilter request)
        {
            IQueryable<BookingGetAllActive> availdable = await _bookingService
                .GetAvailableByFilter(request);

            // to see it
            Expression<Func<ParkSpace, bool>> expression = parkSpace => availdable
                .Any(b => b.ParkSpaceId == parkSpace.Id);

            IQueryable<ParkSpace> parkSpaces = _parkSpaceRepository
                .FindByCriteria(expression);

            var availdableParkSpaces = parkSpaces.Select(parkSpace => new ParkSpaceGetAvaildable
            {
                ParkSpaceId = parkSpace.Id,
                Name = parkSpace.Name
            });

            return await availdableParkSpaces
                .ToListAsync();
        }
    }
}