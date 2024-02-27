using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;

namespace ParkingSpaces.Services
{
    public interface IParkSpaceService
    {
        public Task<IEnumerable<ParkSpaceGetAvaildable>> GetAvailable();

        public Task<IEnumerable<ParkSpaceGetAvaildable>> GetAvailableByFilter(ParkSpaceGetAvailableFilter request);
    }
}
