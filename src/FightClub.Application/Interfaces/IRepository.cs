using FightClub.Application.Specifications;

namespace FightClub.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<int> CountAsync(ISpecification<T> specification);
    Task<bool> AnyAsync(ISpecification<T> specification);    

    Task AddAsync(T entity);
    void Delete(T entity);
    void Update(T entity);

    Task SaveChangesAsync();

    IQueryable<T> Query(ISpecification<T> spec);
}
