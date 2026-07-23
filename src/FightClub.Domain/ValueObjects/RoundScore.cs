using FightClub.Domain.Common;
using FightClub.Domain.Exceptions;

namespace FightClub.Domain.ValueObjects;

public sealed class RoundScore : ValueObject
{
    public int ScoreA { get; }
    public int ScoreB { get; }

    public RoundScore(int scoreA, int scoreB)
    {
        if (scoreA < 0 || scoreB < 0)
        {
            throw new DomainException(
                "Score cannot be negative");
        }

        ScoreA = scoreA;
        ScoreB = scoreB;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ScoreA;
        yield return ScoreB;
    }
}
