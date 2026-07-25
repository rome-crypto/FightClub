using System.Text;
using FightClub.Application.Common.Interfaces.Authentication;
using FightClub.Application.Common.Options;
using FightClub.Application.Interfaces;
using FightClub.Infrastructure.Authentication;
using FightClub.Infrastructure.Persistence;
using FightClub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FightClub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<FightClubDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("Default"));
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Authentication
        // Почему Configure<JwtOptions>: загружаем настройки JWT из appsettings.json
        // Options pattern позволяет инжектить IOptions<JwtOptions> в сервисы
        var jwtSection = configuration.GetSection("JwtOptions");
        services.Configure<JwtOptions>(jwtSection);

        var jwtOptions = jwtSection.Get<JwtOptions>();

        // Регистрируем JWT аутентификацию
        // Почему AddAuthentication: настраиваем ASP.NET Core для работы с JWT
        services.AddAuthentication(options =>
        {
            // Схема по умолчанию - JWT Bearer
            // Это значит что [Authorize] будет искать JWT токен в заголовке Authorization
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // TokenValidationParameters - правила валидации токена
            // Эти же правила используются в JwtProvider.ValidateToken
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,              // проверяем кто выдал токен
                ValidateAudience = true,            // проверяем для кого токен
                ValidateLifetime = true,            // проверяем срок действия
                ValidateIssuerSigningKey = true,    // проверяем подпись
                ValidIssuer = jwtOptions?.Issuer,
                ValidAudience = jwtOptions?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions?.SecretKey ?? string.Empty)),
                ClockSkew = TimeSpan.Zero           // убираем допуск по времени (по умолчанию 5 минут)
                                                   // Почему Zero: токен истекает ровно в указанное время
            };

            // События для логирования ошибок аутентификации
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    // Логируем ошибки валидации токена для debugging
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    // OnChallenge срабатывает когда пользователь пытается
                    // обратиться к защищенному endpoint без токена
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        error = "You are not authorized"
                    });
                    return context.Response.WriteAsync(result);
                }
            };
        });

        // Регистрируем сервисы аутентификации
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
