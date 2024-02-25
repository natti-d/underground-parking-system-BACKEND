using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Response;

namespace ParkingSpaces.Services
{
    public interface IParkSpaceService
    {
        public Task<IEnumerable<ParkSpaceGetAvaildableParkSpacesResponse>> GetAvaildableParkSpaces();
    }
}
