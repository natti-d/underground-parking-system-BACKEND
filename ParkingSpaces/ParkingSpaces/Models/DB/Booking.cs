using ParkingSpaces.Models.Request;

namespace ParkingSpaces.Models.DB
{
    public class Booking : BookingRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ParkSpace ParkSpace { get; set; }
        public DateTime EndTime { get; set; }
    }
}
