using DAL.ApplicationDbContext;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class BaseRepository<T, K> : IBaseRepository<T, K>
        where T : class, IBaseModel<K>
        where K : IEquatable<K>
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly AppMsSqlDbContext _context;
        public BaseRepository(AppMsSqlDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();

        }

        public async Task<T> GetById(K id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> Create(T model, bool commitTransaction = true)
        {
            model.CreateDate = DateTime.UtcNow;
            var entity = (await _dbSet.AddAsync(model)).Entity;
            if (commitTransaction)
                await SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(T model)
        {
            var entity = _context.Remove(model).Entity;
            await SaveChangesAsync();
            return entity;
        }

        public async Task<T> SoftDelete(T model)
        {
            model.DeleteDate = DateTime.UtcNow;
            var entity = _dbSet.Update(model).Entity;
            await SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update(T model)
        {
            model.UpdateDate = DateTime.UtcNow;
            var entity = _dbSet.Update(model).Entity;
            await SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetFilteredListAsync(IQueryable<T> query, bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.Where(x => !x.DeleteDate.HasValue).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetUnFilteredListAsync(IQueryable<T> query, bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
