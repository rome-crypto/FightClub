using FightClub.Application.DTOs.Fights;

namespace FightClub.Application.Interfaces;

public interface IFightSimulationService
{
    Task CancelAsync(Guid fightId);
    Task ExecuteAsync(Guid fightId);
}
