using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSpaces.Models.DB
{
    public class ParkSpace
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
