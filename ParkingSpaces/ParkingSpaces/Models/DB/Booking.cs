namespace ParkingSpaces.Models.DB
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int ParkSpaceId { get; set; }

        public ParkSpace ParkSpace { get; set; }

        public TimeSpan Duration{ get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
