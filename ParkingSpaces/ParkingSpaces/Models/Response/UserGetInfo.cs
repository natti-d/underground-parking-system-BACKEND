using ParkingSpaces.Models.DB;

namespace ParkingSpaces.Models.Response
{
    public class UserGetInfo
    {
        public string Username { get; set; }

        public string Plate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public byte[] Avatar { get; set; }
    }
}
