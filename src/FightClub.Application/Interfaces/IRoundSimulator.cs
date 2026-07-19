using FightClub.Domain.ValueObjects;

namespace FightClub.Application.Interfaces;

public interface IRoundSimulator
{
    public RoundScore Simulate(
        Guid boxerAId,
        Guid boxerBId);
}
