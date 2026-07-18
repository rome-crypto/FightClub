using FightClub.Api.Middleware;
using FightClub.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using FightClub.Application;
using FightClub.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            using var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<FightClubDbContext>();

            db.Database.Migrate();
        }

        app.MapControllers();

        app.Run();
    }
}
