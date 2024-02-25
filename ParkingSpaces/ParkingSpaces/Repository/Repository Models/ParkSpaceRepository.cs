using ParkingSpaces.Models.DB;
using ParkingSpaces.Repository.Repository_Interfaces;

namespace ParkingSpaces.Repository.Repository_Models
{
    public class ParkSpaceRepository : BaseRepository<ParkSpace>, IParkSpaceRepository
    {
        public ParkSpaceRepository(ParkingSpacesDbContext repository_Context) 
            : base(repository_Context)
        {
        }
    }
}
