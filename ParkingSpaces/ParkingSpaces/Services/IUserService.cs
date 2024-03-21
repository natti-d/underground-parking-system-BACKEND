using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using System.Security.Claims;

namespace ParkingSpaces.Services
{
    public interface IUserService
    {
        public Task<string> Login(UserLogin request);

        public Task Register(UserRequest request);

        public Task Delete(ClaimsPrincipal user);

        public Task Update(UserRequest request, ClaimsPrincipal user);

        public Task<UserGetInfo> GetInfo(ClaimsPrincipal user);

        public Task SetUpAvatar(ClaimsPrincipal user, IFormFile file);
    }
}
