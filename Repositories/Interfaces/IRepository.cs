using System.Linq.Expressions;
namespace FightClub.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? criteria);

    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T entity);
    void Delete(T entity);
    void Update(T entity);

    Task SaveChangesAsync();
}
