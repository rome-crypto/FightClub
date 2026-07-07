using FightClub.Domain.Enums;

namespace FightClub.Domain.Services;

internal interface IRatingPolicy
{
    int CalculateNewRating(
        int currentRating,
        int opponentRating,
        FightResult result,
        int kFactor = 32);
}
