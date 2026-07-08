using FightClub.Application.DTOs.Fights;

namespace FightClub.Application.Interfaces;

public interface IFightSimulationService
{
    Task<FightResponseDto> ExecuteAsync(Guid fightId);
}
