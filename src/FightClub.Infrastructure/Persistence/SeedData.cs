using FightClub.Domain.Entities.Auth;
using FightClub.Domain.Enums;
using FightClub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FightClub.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        FightClubDbContext context = scope.ServiceProvider.GetRequiredService<FightClubDbContext>();
        ILogger<FightClubDbContext> logger = scope.ServiceProvider.GetRequiredService<ILogger<FightClubDbContext>>();

        try
        {
            await SeedRolesAsync(context, logger);
            await SeedAdminUserAsync(context, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private static async Task SeedRolesAsync(FightClubDbContext context, ILogger logger)
    {
        if (await context.Roles.AnyAsync())
        {
            logger.LogInformation("Roles already exist. Skipping seed.");
            return;
        }

        logger.LogInformation("Seeding roles...");

        var roles = new List<Role>
        {
            new("Admin"),
            new("Manager"),
            new("User")
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();

        logger.LogInformation("Roles seeded successfully.");
    }

    private static async Task SeedAdminUserAsync(FightClubDbContext context, ILogger logger)
    {
        if (await context.Users.AnyAsync(u => u.Email == "admin@fightclub.com"))
        {
            logger.LogInformation("Admin user already exists. Skipping seed.");
            return;
        }

        logger.LogInformation("Seeding admin user...");

        // Хешируем пароль
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!", 12);

        // Создаем админа
        var admin = new User(
            "admin",
            "admin@fightclub.com",
            passwordHash);

        // Получаем роль Admin
        Role? adminRole = await context.Roles
            .FirstOrDefaultAsync(r => r.Name == "Admin");

        if (adminRole != null)
        {
            admin.AddRole(adminRole);
        }

        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();

        logger.LogInformation("Admin user seeded successfully.");
        logger.LogInformation("Email: admin@fightclub.com");
        logger.LogInformation("Password: Admin123!");
    }
}
