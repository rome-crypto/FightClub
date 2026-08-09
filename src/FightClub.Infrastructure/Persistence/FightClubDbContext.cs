using FightClub.Domain.Entities;
using FightClub.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Infrastructure.Persistence;

public class FightClubDbContext(DbContextOptions<FightClubDbContext> options)
    : DbContext(options)
{
    public DbSet<Boxer> Boxers => Set<Boxer>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Fight> Fights => Set<Fight>();

    // Auth tables
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FightClubDbContext).Assembly);
    }
}
