namespace ParkingSpaces.Services
{
    public interface IAuthService
    {
        string CreateToken(string username, string password);
        int GetUserIdFromToken(string token);
    }
}
