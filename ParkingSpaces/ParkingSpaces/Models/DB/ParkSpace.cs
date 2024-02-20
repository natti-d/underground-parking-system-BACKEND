using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSpaces.Models.DB
{
    public class ParkSpace
    {
        // to not be primary key
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        // to be relation and be the name of the user?
        public string Name { get; set; }
    }
}
