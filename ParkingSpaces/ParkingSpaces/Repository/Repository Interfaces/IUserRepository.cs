using ParkingSpaces.Models.DB;
using System.Linq.Expressions;

namespace ParkingSpaces.Repository.Repository_Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> FindAny(Expression<Func<User, bool>> expression);
    }
}
