using FightClub.Application.Interfaces;

namespace FightClub.Infrastructure.Persistence;

public class UnitOfWork(FightClubDbContext context) : IUnitOfWork
{
    private readonly FightClubDbContext _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
