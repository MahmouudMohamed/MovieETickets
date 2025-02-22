using Microsoft.EntityFrameworkCore;
using MovieETickets.Data;
using MovieETickets.Repositories.IRepositories;
using System.Linq.Expressions;

namespace MovieETickets.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();
        public DbSet<T> dbSet;

        public Repository()
        {
            dbSet = _dbContext.Set<T>();
        }

        // CRUD
        public void Create(T entity)
        {
            dbSet.Add(entity);
        }
        public void CreateAll(List<T> entities)
        {
            dbSet.AddRange(entities);
        }
        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public void DeleteAll(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            if (tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            return Get(filter, includes, tracked).FirstOrDefault();
        }
    }
}
