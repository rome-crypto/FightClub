using FightClub.Repositories.Interfaces;
using FightClub.Data;
using Microsoft.EntityFrameworkCore;
using FightClub.Specifications;
using FightClub.DTOs.Common;
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

    public async Task<List<T>> GetAllAsync(ISpecification<T> specification)
    {
        var query = SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);

        return await query.ToListAsync();
    }

    public async Task<PagedResult<T>> GetPagedAsync(ISpecification<T> specification)
    {
        var baseQuery = _dbSet.AsQueryable();

        var filteredQuery = baseQuery;

        if (specification.Criteria != null)
            filteredQuery = filteredQuery.Where(specification.Criteria);

        var totalCount = await filteredQuery.CountAsync();
        
        var itemsQuery = SpecificationEvaluator.GetQuery(baseQuery, specification);
        var items = await itemsQuery.ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            Page = specification.Skip / specification.Take + 1,
            PageSize = specification.Take,
            TotalCount = totalCount
        };
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

    public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        var query = SpecificationEvaluator.GetCountQuery(
            _dbSet.AsQueryable(), 
            specification);

        return await query.CountAsync();
    }

    public async Task<bool> AnyAsync(ISpecification<T> specification)
    {
        var query = SpecificationEvaluator.GetCountQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.AnyAsync();
    }
}
