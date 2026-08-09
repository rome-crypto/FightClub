namespace FightClub.Application.Interfaces;

public interface IFightSimulationService
{
    public Task CancelAsync(Guid fightId, CancellationToken cancellationToken);
    public Task ExecuteAsync(Guid fightId, CancellationToken cancellationToken);
}
