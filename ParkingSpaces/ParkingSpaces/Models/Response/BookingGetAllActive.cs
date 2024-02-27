namespace ParkingSpaces.Models.NewFolder
{
    public class BookingGetAllActive
    {
        public int BookingId { get; set; }
        public int ParkSpaceId { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
