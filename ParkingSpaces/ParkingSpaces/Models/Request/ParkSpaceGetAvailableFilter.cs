namespace ParkingSpaces.Models.Request
{
    public class ParkSpaceGetAvailableFilter
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
