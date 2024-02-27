namespace ParkingSpaces.Models.Request
{
    public class BookingUpdate
    {
        public int BookingId { get; set; }
        public int ParkSpaceId { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
    }
}
