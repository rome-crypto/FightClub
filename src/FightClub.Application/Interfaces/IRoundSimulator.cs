using FightClub.Domain.ValueObjects;

namespace FightClub.Application.Interfaces;

public interface IRoundSimulator
{
    RoundScore Simulate(
        Guid boxerAId,
        Guid boxerBId);
}