using ParkingSpaces.Models.Request;

namespace ParkingSpaces.Models.DB
{
    public class User : UserRequest
    {
        public int Id { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
