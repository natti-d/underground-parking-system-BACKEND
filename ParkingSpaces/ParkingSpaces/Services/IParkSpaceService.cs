using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;

namespace ParkingSpaces.Services
{
    public interface IParkSpaceService
    {
        public Task<IEnumerable<ParkSpaceResponse>> GetAvailable();

        public Task<IEnumerable<ParkSpaceResponse>> GetAvailableByFilter(ParkSpaceGetAvailableByFilter request);
        public Task<ParkSpaceResponse> GetById(int parkSpaceId);
    }
}
