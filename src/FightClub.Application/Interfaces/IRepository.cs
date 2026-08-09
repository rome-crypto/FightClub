using FightClub.Application.Specifications.Common;

namespace FightClub.Application.Interfaces;

public interface IRepository<T> where T : class
{
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken);
    public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken);

    public Task AddAsync(T entity, CancellationToken cancellationToken);
    public void Delete(T entity);
    public void Update(T entity);

    public Task SaveChangesAsync(CancellationToken cancellationToken);

    public IQueryable<T> Query(ISpecification<T> spec);
}
