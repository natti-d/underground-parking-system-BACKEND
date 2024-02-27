using ParkingSpaces.Models.Request;

namespace ParkingSpaces.Models.NewFolder
{
    public class BookingResponse : BookingRequest
    {
        public int BookingId { get; set; }
        public DateTime EndTime { get; set; }
    }
}
