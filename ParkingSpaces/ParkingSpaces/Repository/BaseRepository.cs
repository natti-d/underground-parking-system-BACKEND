﻿using ParkingSpaces.Repository.Repository_Interfaces;
using System.Linq.Expressions;

namespace ParkingSpaces.Repository
{
    // NOTE: define what we can make with the db
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public ParkingSpacesDbContext RepositoryContext { get; set; }

        public BaseRepository(ParkingSpacesDbContext repository_Context)
        {
            RepositoryContext = repository_Context;
        }

        // filter by criteria
        public virtual IQueryable<T> FindByCriteria(Expression<Func<T, bool>> expression)
            => RepositoryContext.Set<T>().Where(expression);

        // more good for quiring data
        public virtual T FindById(int id)
            => RepositoryContext.Set<T>().Find(id);

        public virtual IQueryable<T> FindAll() => RepositoryContext.Set<T>();

        public virtual async Task Create(T entity)
        {
            await RepositoryContext.Set<T>().AddAsync(entity);
            await RepositoryContext.SaveChangesAsync();
        }

        public virtual async Task Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
            await RepositoryContext.SaveChangesAsync();
        }

        public virtual async Task Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
            await RepositoryContext.SaveChangesAsync();
        }

        public virtual async Task SaveChanges() => await RepositoryContext.SaveChangesAsync();
    }
}
