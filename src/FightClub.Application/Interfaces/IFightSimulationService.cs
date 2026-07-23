namespace FightClub.Application.Interfaces;

public interface IFightSimulationService
{
    public Task CancelAsync(Guid fightId);
    public Task ExecuteAsync(Guid fightId);
}
