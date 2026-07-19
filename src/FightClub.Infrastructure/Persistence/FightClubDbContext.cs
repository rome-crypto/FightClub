using FightClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Infrastructure.Persistence;

public class FightClubDbContext(DbContextOptions<FightClubDbContext> options) 
    : DbContext(options)
{
    public DbSet<Boxer> Boxers => Set<Boxer>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Fight> Fights => Set<Fight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FightClubDbContext).Assembly);
    }
}
