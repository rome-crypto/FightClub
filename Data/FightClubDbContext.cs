using FightClub.Entities;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Data;

public class FightClubDbContext : DbContext
{
    public FightClubDbContext(DbContextOptions<FightClubDbContext> options)
        : base(options)
    {
    }

    public DbSet<Boxer> Boxers => Set<Boxer>();
}
