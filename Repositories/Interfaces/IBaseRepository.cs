using DAL.Interfaces;

namespace Repositories.Interfaces;


public interface IBaseRepository<T, K>
where T : class, IBaseModel<K>
where K : IEquatable<K>
{
    public Task<T> GetById(K Id);
    public Task<T> Create(T model, bool commitTransaction);
    public Task<T> Delete(T model);
    public Task<T> SoftDelete(T model);
    public Task<T> Update(T model);
    public Task<IEnumerable<T>> GetFilteredListAsync(IQueryable<T> query, bool asNoTracking);
    public Task<int> SaveChangesAsync();
}
