using FightClub.DTOs.Common;
using FightClub.Specifications;
using System.Linq.Expressions;
namespace FightClub.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllAsync(ISpecification<T> specification);
    Task<PagedResult<T>> GetPagedAsync(ISpecification<T> specification);
    Task<T?> FirstOrDefaultAsync(ISpecification<T> specification);
    Task<int> CountAsync(ISpecification<T> specification);
    Task<bool> AnyAsync(ISpecification<T> specification);    

    Task AddAsync(T entity);
    void Delete(T entity);
    void Update(T entity);

    Task SaveChangesAsync();

    IQueryable<T> Query(BaseSpecification<T> spec);
}
