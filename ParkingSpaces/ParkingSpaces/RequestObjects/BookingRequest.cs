namespace ParkingSpaces.RequestObjects
{
    public class BookingRequest
    {
        // or park space name?
        public int ParkSpaceId { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }
    }
}
