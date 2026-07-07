using FightClub.Domain.Enums;
using FightClub.Domain.Exceptions;

namespace FightClub.Domain.Services;

public class EloRatingPolicy : IRatingPolicy
{
    public int CalculateNewRating(
        int currentRating, 
        int opponentRating, 
        FightResult result, 
        int kFactor = 32)
    {
        double actualScore = result switch
        {
            FightResult.Win => 1.0,
            FightResult.Loss => 0.0,
            FightResult.Draw => 0.5,
            _ => throw new DomainException("Invalid fight result")
        };

        double expectedScore = 1.0 / (1.0 + Math.Pow(10, (opponentRating - currentRating) / 400.0));

        int newRating = (int)Math.Round(
            currentRating + kFactor * (actualScore - expectedScore));

        return Math.Max(0, newRating);
    }
}
