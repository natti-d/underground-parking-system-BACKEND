using Microsoft.EntityFrameworkCore;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using System.Linq.Expressions;
using System.Xml.Linq;

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
        public virtual async Task<IEnumerable<ParkSpaceResponse>> GetAvailable()
        {
            IQueryable<BookingResponse> activeBookings = await _bookingService
                .GetActiveForNow();

            Expression<Func<ParkSpace, bool>> expression = parkSpace => !activeBookings
                .Any(b => b.ParkSpaceId == parkSpace.Id);

            IQueryable<ParkSpace> parkSpaces = _parkSpaceRepository
                .FindByCriteria(expression);

            var availdableParkSpaces = parkSpaces.Select(parkSpace => new ParkSpaceResponse
            {
                ParkSpaceId = parkSpace.Id,
                Name = parkSpace.Name
            });

            return await availdableParkSpaces
                .ToListAsync();
        }

        // get all available park spaces by filter (from, to)
        public virtual async Task<IEnumerable<ParkSpaceResponse>> GetAvailableByFilter(ParkSpaceGetAvailableByFilter request)
        {
            IQueryable<int> parkSpaceIds = (await _bookingService
                .GetAvailableByFilter(request))
                .Select(b => b.ParkSpaceId)
                .Distinct();

            
            var parkSpaces = new List<ParkSpaceResponse>();
            foreach (var parkSpaceId in parkSpaceIds)
            {
                // 16 * 4 miliseconds
                var parkSpaceName = (await _parkSpaceRepository
                    .FindById(parkSpaceId)).Name;

                parkSpaces.Add(new ParkSpaceResponse()
                {
                    ParkSpaceId = parkSpaceId,
                    Name = parkSpaceName
                });
            }

            Expression<Func<ParkSpace, bool>> expression = parkSpace =>
                parkSpace.Bookings.Count == 0;

            IEnumerable<ParkSpace> emptyParkSpaces = await _parkSpaceRepository
                .FindByCriteria(expression)
                .ToListAsync();

            foreach (var space in emptyParkSpaces)
            {
                parkSpaces.Add(new ParkSpaceResponse()
                {
                    ParkSpaceId = space.Id,
                    Name = space.Name
                });
            }

            return parkSpaces;
        }

        public virtual async Task<ParkSpaceResponse> GetById(int parkSpaceId)
        {
            ParkSpace parkSpace = await _parkSpaceRepository
                .FindById(parkSpaceId);

            return new ParkSpaceResponse() { ParkSpaceId = parkSpace.Id, Name = parkSpace.Name, };
        }
    }
}