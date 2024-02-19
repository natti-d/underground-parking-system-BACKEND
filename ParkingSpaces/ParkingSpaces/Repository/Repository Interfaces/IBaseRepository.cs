using System.Linq.Expressions;

namespace ParkingSpaces.Repository.Repository_Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> FindByCriteria(Expression<Func<T, bool>> expression);

        Task Create(T entity);

        Task Update(T entity);

        Task Delete(T entity);

        Task SaveChanges();

    }
}
