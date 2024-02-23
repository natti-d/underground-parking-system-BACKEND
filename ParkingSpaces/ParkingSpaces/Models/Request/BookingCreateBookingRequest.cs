namespace ParkingSpaces.Models.Request
{
    public class BookingCreateBookingRequest
    {
        public int ParkSpaceId { get; set; }

        // "duration": "02:30:00" 
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
    }
}
