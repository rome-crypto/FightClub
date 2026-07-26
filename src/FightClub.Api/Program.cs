using System.Reflection;
using FightClub.Api.Middleware;
using FightClub.Application;
using FightClub.Infrastructure;
using FightClub.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FightClub.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Controllers
        builder.Services.AddControllers();

        // FluentValidation
        builder.Services
            .AddFluentValidationAutoValidation();

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FightClub API",
                Version = "v1",
                Description = "Boxing match management system"
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            // Добавляем JWT авторизацию в Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT токен в формате: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                Array.Empty<string>()
                }
            });
        });

        // Dependency Injection
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        // Health Checks
        builder.Services.AddHealthChecks()
            .AddNpgSql(
                builder.Configuration.GetConnectionString("Default")!,
                name: "PostgreSQL",
                tags: ["db", "postgresql"])
            .AddDbContextCheck<FightClubDbContext>(
                name: "FightClub EF Core",
                tags: ["db", "efcore"]);

        WebApplication app = builder.Build();

        // Exeption handling middleware
        app.UseMiddleware<ExceptionMiddleware>();

        //app.UseHttpsRedirection();

        // Swagger and database migration in development environment
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            using IServiceScope scope = app.Services.CreateScope();

            FightClubDbContext db = scope.ServiceProvider.GetRequiredService<FightClubDbContext>();

            db.Database.Migrate();

            await SeedData.InitializeAsync(scope.ServiceProvider);
        }

        app.MapHealthChecks("/health");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}
