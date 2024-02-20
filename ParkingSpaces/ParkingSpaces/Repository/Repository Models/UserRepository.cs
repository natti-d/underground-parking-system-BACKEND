using ParkingSpaces.Models.DB;
using ParkingSpaces.Repository.Repository_Interfaces;

namespace ParkingSpaces.Repository.Repository_Models
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        // NOTE: DI plays with the instances
        public UserRepository(ParkingSpacesDbContext repository_Context)
            : base(repository_Context)
        {
        }
    }
}
