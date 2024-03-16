namespace ParkingSpaces.Services
{
    public interface IPasswordHasherService
    {
        public string Hash(string password);
        public bool Verify(string passwordHash, string inputPassword);
    }
}
