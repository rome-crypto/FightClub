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
    public DbSet<Trainer> Trainers => Set<Trainer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Boxer>()
            .HasOne(x => x.Trainer)
            .WithMany(x => x.Boxers)
            .HasForeignKey(x => x.TrainerId);
    }
}
