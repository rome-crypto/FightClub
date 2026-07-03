using FightClub.Data;
using FightClub.Engine;
using FightClub.Mappings;
using FightClub.Middleware;
using FightClub.Repositories;
using FightClub.Repositories.Interfaces;
using FightClub.Services;
using FightClub.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IFightService, FightService>();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IBoxerRepository, BoxerRepository>();

            builder.Services.AddScoped<IFightEngine, FightEngine>();

            builder.Services.AddAutoMapper(typeof(BoxerProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(TrainerProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(FightProfile).Assembly);

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            
            app.UseAuthorization();

            app.UseRouting();

            app.MapControllers();
            
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
