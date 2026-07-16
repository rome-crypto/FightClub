using FightClub.Domain.Entities;
using FightClub.Domain.Services;
using FightClub.Domain.ValueObjects;

namespace FightClub.Domain.Policies;

public sealed class BoxingFightEndingPolicy : IFightEndingPolicy
{
    public FightOutcome Evaluate(
        IReadOnlyCollection<FightRound> rounds, 
        Guid boxerAId, 
        Guid boxerBId, 
        int plannedRounds)
    {
        if (rounds.Count(r => r.IsFinished) < plannedRounds)
            return FightOutcome.Continue();

        int totalScoreA = rounds.Sum(r => r.ScoreA);
        int totalScoreB = rounds.Sum(r => r.ScoreB);

        if (totalScoreA == totalScoreB)
        {
            return FightOutcome.Finish(
                null,
                FightEndType.Draw);
        }

        return FightOutcome.Finish(
            totalScoreA > totalScoreB
                ? boxerAId
                : boxerBId,
            FightEndType.Decision);
    }
}