namespace ParkingSpaces.Models.Request
{
    public class ParkSpaceGetAvailableByFilter
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
