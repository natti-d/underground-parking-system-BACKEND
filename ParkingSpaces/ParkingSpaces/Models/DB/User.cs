using ParkingSpaces.Models.Request;
using System.Diagnostics.CodeAnalysis;

namespace ParkingSpaces.Models.DB
{
    public class User : UserRequest
    {
        public int Id { get; set; }

        public byte[]? Avatar { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
