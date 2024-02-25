using ParkingSpaces.Models.DB;

namespace ParkingSpaces.Models.Request
{
    public class UserUpdateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Plate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
