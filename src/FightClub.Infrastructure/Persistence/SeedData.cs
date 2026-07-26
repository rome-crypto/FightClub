using FightClub.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FightClub.Infrastructure.Persistence;

public static class SeedData
{
    private const string AdminEmail = "admin@fightclub.com";
    private const string AdminPassword = "Admin123!";

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        FightClubDbContext context = scope.ServiceProvider.GetRequiredService<FightClubDbContext>();
        ILogger<FightClubDbContext> logger = scope.ServiceProvider.GetRequiredService<ILogger<FightClubDbContext>>();

        try
        {
            await SeedRolesAsync(context);
            await SeedAdminUserAsync(context);

            logger.LogInformation("Database seeded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database seeding failed");
            throw;
        }
    }

    private static async Task SeedRolesAsync(FightClubDbContext context)
    {
        if (await context.Roles.AnyAsync())
        {
            return;
        }

        var roles = new List<Role>
        {
            new("Admin"),
            new("Manager"),
            new("User")
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAdminUserAsync(FightClubDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.Email == AdminEmail))
        {
            return;
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(AdminPassword, 12);

        var admin = new User(
            "admin",
            AdminEmail,
            passwordHash);

        Role? adminRole = await context.Roles
            .FirstOrDefaultAsync(r => r.Name == "Admin");

        if (adminRole != null)
        {
            admin.AddRole(adminRole);
        }

        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();
    }
}
