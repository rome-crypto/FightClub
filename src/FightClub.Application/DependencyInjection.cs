using System.Reflection;
using FightClub.Application.Interfaces;
using FightClub.Application.Services;
using FightClub.Domain.Policies;
using FightClub.Domain.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FightClub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(cfg => { }, assembly);

        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IFightService, FightService>();
        services.AddScoped<IFightSimulationService, FightSimulationService>();
        services.AddScoped<IBoxerService, BoxerService>();
        services.AddScoped<IRoundSimulator, RandomRoundSimulator>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IFightResultService, FightResultService>();
        services.AddScoped<IFightEndingPolicy, BoxingFightEndingPolicy>();
        services.AddScoped<IRatingPolicy, EloRatingPolicy>();

        return services;
    }

}
