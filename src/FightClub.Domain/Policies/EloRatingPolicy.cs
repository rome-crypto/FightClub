using FightClub.Domain.Entities;

namespace FightClub.Domain.Policies;

public sealed class EloRatingPolicy : IRatingPolicy
{
    private const int K = 32;

    public (int WinnerRating, int LoserRating) Calculate(
        Boxer winner,
        Boxer loser)
    {
        double expectedWinner =
            1.0 /
            (1.0 + Math.Pow(10,
                (loser.Ranking.EloRating - winner.Ranking.EloRating) / 400.0));

        double expectedLoser = 1 - expectedWinner;

        int winnerRating =
            winner.Ranking.EloRating +
            (int)(K * (1 - expectedWinner));

        int loserRating =
            loser.Ranking.EloRating +
            (int)(K * (0 - expectedLoser));

        return (
            Math.Max(0, winnerRating),
            Math.Max(0, loserRating));
    }

    public (int BoxerARating, int BoxerBRating) CalculateDraw(
        Boxer boxerA,
        Boxer boxerB)
    {
        double expectedA =
            1.0 /
            (1.0 + Math.Pow(10,
                (boxerB.Ranking.EloRating - boxerA.Ranking.EloRating) / 400.0));

        double expectedB = 1 - expectedA;

        int ratingA =
            boxerA.Ranking.EloRating +
            (int)(K * (0.5 - expectedA));

        int ratingB =
            boxerB.Ranking.EloRating +
            (int)(K * (0.5 - expectedB));

        return (
            Math.Max(0, ratingA),
            Math.Max(0, ratingB));
    }
}
