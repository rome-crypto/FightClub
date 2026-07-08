using FightClub.Domain.Enums;
using FightClub.Domain.Common;

namespace FightClub.Domain.ValueObjects;

public sealed class FightOutcome : ValueObject
{
    public bool IsFinished { get; }

    public Guid? WinnerId { get; }

    public FightEndType? EndType { get; }

    private FightOutcome(
        bool isFinished,
        Guid? winnerId,
        FightEndType? endType)
    {
        IsFinished = isFinished;
        WinnerId = winnerId;
        EndType = endType;
    }

    public static FightOutcome Continue()
        => new(false, null, null);

    public static FightOutcome Finish(
        Guid? winnerId,
        FightEndType endType)
        => new(true, winnerId, endType);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return IsFinished;
        yield return WinnerId;
        yield return EndType;
    }
}
