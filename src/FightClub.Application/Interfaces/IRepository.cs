using FightClub.Application.Specifications.Common;

namespace FightClub.Application.Interfaces;

public interface IRepository<T> where T : class
{
    public Task<T?> GetByIdAsync(Guid id);
    public Task<int> CountAsync(ISpecification<T> specification);
    public Task<bool> AnyAsync(ISpecification<T> specification);

    public Task AddAsync(T entity);
    public void Delete(T entity);
    public void Update(T entity);

    public Task SaveChangesAsync();

    public IQueryable<T> Query(ISpecification<T> spec);
}
