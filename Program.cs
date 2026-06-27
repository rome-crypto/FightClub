using FightClub.Data;
using FightClub.Repositories;
using FightClub.Repositories.Interfaces;
using FightClub.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using FightClub.Middleware;

namespace FightClub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FightClubDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<IBoxerService, BoxerService>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IBoxerRepository, BoxerRepository>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            
            app.UseAuthorization();
            
            app.MapControllers();
            
            app.UseRouting();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FightClub API v1");
                    c.RoutePrefix = string.Empty;
                });

                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FightClubDbContext>();
                db.Database.Migrate();
            }

           
            app.Run();
        }
    }
}
