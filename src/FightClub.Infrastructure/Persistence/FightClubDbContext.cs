using FightClub.Entities;
using FightClub.Entities.Fight;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Data;

public class FightClubDbContext : DbContext
{
    public FightClubDbContext(DbContextOptions<FightClubDbContext> options)
        : base(options)
    {
    }

    public DbSet<Boxer> Boxers => Set<Boxer>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Fight> Fights => Set<Fight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FightClubDbContext).Assembly);
    }
}
