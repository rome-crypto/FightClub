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

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = SpecificationEvaluator.GetCountQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = SpecificationEvaluator.GetCountQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.AnyAsync(cancellationToken);
    }

    public IQueryable<T> Query(ISpecification<T> spec)
    {
        return SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), spec);
    }
}
