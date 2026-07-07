using FightClub.Domain.Entities;
using FightClub.Domain.Enums;

namespace FightClub.Domain.Policies;

public class BoxingFightEndingPolicy : IFightEndingPolicy
{
    public bool TryFinish(
        Fight fight,
        out Guid? winnerId,
        out FightEndType endType)
    {
        winnerId = null;
        endType = default;


        if (fight.ActualRounds < fight.PlannedRounds)
        {
            return false;
        }


        if (fight.TotalScoreA == fight.TotalScoreB)
        {
            endType = FightEndType.Draw;
            return true;
        }


        winnerId =
            fight.TotalScoreA > fight.TotalScoreB
                ? fight.BoxerAId
                : fight.BoxerBId;


        endType = FightEndType.Decision;

        return true;
    }
}