using FightClub.Repositories.Interfaces;
using FightClub.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace FightClub.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly FightClubDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(FightClubDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate)
    {
        var query = _dbSet.AsQueryable();

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
