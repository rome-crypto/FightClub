using FightClub.Domain.Entities;

namespace FightClub.Domain.Policies;

public sealed class EloRatingPolicy : IRatingPolicy
{
    private const int K = 32;


    public (int BoxerARating,
        int BoxerBRating) 
        Calculate(Boxer boxerA,
        Boxer boxerB,
        Guid? winnerId)
    {
        double expectedA =
            1.0 /
            (
                1 +
                Math.Pow(
                    10,
                    (boxerB.Ranking.EloRating -
                     boxerA.Ranking.EloRating)
                    / 400.0)
            );


        double expectedB = 1 - expectedA;



        double scoreA;
        double scoreB;


        if (winnerId == null)
        {
            scoreA = 0.5;
            scoreB = 0.5;
        }
        else if (winnerId == boxerA.Id)
        {
            scoreA = 1;
            scoreB = 0;
        }
        else
        {
            scoreA = 0;
            scoreB = 1;
        }



        int newA =
            boxerA.Ranking.EloRating +
            (int)(K * (scoreA - expectedA));


        int newB =
            boxerB.Ranking.EloRating +
            (int)(K * (scoreB - expectedB));



        return (
            Math.Max(0, newA),
            Math.Max(0, newB));
    }
}