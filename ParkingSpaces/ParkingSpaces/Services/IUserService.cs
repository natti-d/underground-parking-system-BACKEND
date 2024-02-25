using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;

namespace ParkingSpaces.Services
{
    public interface IUserService
    {
        public Task Login(UserLoginRequest request);

        public Task Register(UserRegisterRequest request);

        public Task Delete(int userId);

        public Task Update(UserUpdateRequest request, int userId);

        public Task<UserGetInfoResponse> GetInfo(int userId);
    }
}
