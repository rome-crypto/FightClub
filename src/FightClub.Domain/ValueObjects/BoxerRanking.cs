using FightClub.Domain.Common;
using FightClub.Domain.Enums;
using FightClub.Domain.Exceptions;
using System.Data;

namespace FightClub.Domain.ValueObjects;

public sealed class BoxerRanking : ValueObject
{
    public int EloRating { get; private set; }
    public int RankingPoints {  get; private set; }

    internal BoxerRanking()
    {
        EloRating = 1500;
    }

    internal void UpdateElo(int rating)
    {
        EloRating = Math.Max(0, rating);
    }
    internal void AddRankingPoints(int points)
    {
        if (points < 0)
            throw new DomainException("Points cannot be less than 0");

        RankingPoints += points;
    }
    internal void RemoveRankingPoints(int points)
    {
        if (points < 0)
            throw new DomainException("Points cannot be less than 0");

        RankingPoints = Math.Max(0, RankingPoints - points);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EloRating;
        yield return RankingPoints;
    }
}
