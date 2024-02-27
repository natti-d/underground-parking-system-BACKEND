using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;

namespace ParkingSpaces.Services
{
    public interface IUserService
    {
        public Task Login(UserLogin request);

        public Task Register(UserRequest request);

        public Task Delete(int userId);

        public Task Update(UserRequest request, int userId);

        public Task<UserGetInfo> GetInfo(int userId);
    }
}
