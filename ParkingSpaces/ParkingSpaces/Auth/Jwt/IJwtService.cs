using System.Security.Claims;

namespace ParkingSpaces.Auth.Jwt
{
    public interface IJwtService
    {
        public int GetUserIdFromToken(ClaimsPrincipal user);
        public string Generate(int userId);
    }
}
