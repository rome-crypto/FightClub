using FightClub.Application.Interfaces;
using FightClub.Application.Specifications.Common;
using FightClub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace FightClub.Infrastructure.Repositories;

public class Repository<T>(FightClubDbContext context)
    : IRepository<T> where T : class
{
    private readonly FightClubDbContext _context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
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

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        IQueryable<T> query = SpecificationEvaluator.GetCountQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.CountAsync();
    }

    public async Task<bool> AnyAsync(ISpecification<T> specification)
    {
        IQueryable<T> query = SpecificationEvaluator.GetCountQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.AnyAsync();
    }

    public IQueryable<T> Query(ISpecification<T> spec)
    {
        return SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), spec);
    }
}
