using FightClub.Entities;
using FightClub.Data;
using FightClub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace FightClub.Repositories;

public class BoxerRepository : Repository<Boxer>, IBoxerRepository
{
    public BoxerRepository(FightClubDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Boxer>> GetByWeightCategoryAsync(string category)
    {
        return await _dbSet
            .Where(x => x.WeightCategory == category)
            .ToListAsync();
    }

    public async Task<List<Boxer>> GetByAgeAsync(int age)
    {
        return await _dbSet
            .Where(x => x.Age == age)
            .ToListAsync();
    }
}
