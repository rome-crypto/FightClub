using FightClub.Domain.Entities;
using FightClub.Domain.ValueObjects;

namespace FightClub.Domain.Policies;

public interface IFightEndingPolicy
{
    FightOutcome Evaluate(
        IReadOnlyCollection<FightRound> rounds,
        Guid boxerAId,
        Guid boxerBId,
        int plannedRounds);
}