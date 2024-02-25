using Microsoft.EntityFrameworkCore;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Repository.Repository_Interfaces;
using System.Linq.Expressions;

namespace ParkingSpaces.Repository.Repository_Models
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        // NOTE: DI plays with the instances
        public UserRepository(ParkingSpacesDbContext repository_Context)
            : base(repository_Context)
        {
        }

        public async Task<bool> FindAny(Expression<Func<User, bool>> expression)
        {
            return await RepositoryContext.Users.AnyAsync(expression);
        }
    }
}
