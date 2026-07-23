using FightClub.Domain.Common;
using FightClub.Domain.Exceptions;

namespace FightClub.Domain.ValueObjects;

public sealed class BoxerRanking : ValueObject
{
    public int EloRating { get; }
    public int RankingPoints { get; }

    internal BoxerRanking()
    {
        EloRating = 1500;
        RankingPoints = 0;
    }

    private BoxerRanking(int eloRating, int rankingPoints)
    {
        EloRating = eloRating;
        RankingPoints = rankingPoints;
    }

    internal BoxerRanking UpdateElo(int rating)
    {
        return new BoxerRanking(
            eloRating: Math.Max(0, rating),
            rankingPoints: RankingPoints
        );
    }

    internal BoxerRanking AddRankingPoints(int points)
    {
        if (points < 0)
        {
            throw new DomainException("Points cannot be less than 0");
        }

        return new BoxerRanking(
            eloRating: EloRating,
            rankingPoints: RankingPoints + points
        );
    }

    internal BoxerRanking RemoveRankingPoints(int points)
    {
        if (points < 0)
        {
            throw new DomainException("Points cannot be less than 0");
        }

        return new BoxerRanking(
            eloRating: EloRating,
            rankingPoints: Math.Max(0, RankingPoints - points)
        );
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EloRating;
        yield return RankingPoints;
    }
}
