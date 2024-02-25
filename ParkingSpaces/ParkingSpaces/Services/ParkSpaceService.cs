using Microsoft.EntityFrameworkCore;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.NewFolder;
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

        public virtual async Task<IEnumerable<ParkSpaceGetAvaildableParkSpacesResponse>> GetAvaildableParkSpaces()
        {
            IQueryable<BookingGetActiveBookingsResponse> activeBookings = await _bookingService
                .GetActiveBookingsForNow();

            Expression<Func<ParkSpace, bool>> expression = parkSpace => !activeBookings
                .Any(b => b.ParkSpaceId == parkSpace.Id);

            IQueryable<ParkSpace> parkSpaces = _parkSpaceRepository
                .FindByCriteria(expression);

            var availdableParkSpaces = parkSpaces.Select(parkSpace => new ParkSpaceGetAvaildableParkSpacesResponse
            {
                ParkSpaceId = parkSpace.Id,
                Name = parkSpace.Name
            });

            return await availdableParkSpaces
                .ToListAsync();
        }
    }
}