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
    public DbSet<Fight> Fights => Set<Fight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Boxer>()
            .HasOne(x => x.Trainer)
            .WithMany(x => x.Boxers)
            .HasForeignKey(x => x.TrainerId);

        modelBuilder.Entity<Fight>()
            .HasOne(x => x.BoxerA)
            .WithMany(x => x.FightsAsA)
            .HasForeignKey(x => x.BoxerAId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Fight>()
            .HasOne(x => x.BoxerB)
            .WithMany(x => x.FightsAsB)
            .HasForeignKey(x => x.BoxerBId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Fight>()
            .HasOne(x => x.Winner)
            .WithMany()
            .HasForeignKey(x => x.WinnerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
