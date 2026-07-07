using FightClub.Domain.Enums;

namespace FightClub.Domain.Polices;

internal interface IRatingPolicy
{
    int CalculateNewRating(
        int currentRating,
        int opponentRating,
        FightResult result,
        int kFactor = 32);
}
