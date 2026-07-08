using FightClub.Domain.Common;
using FightClub.Domain.Exceptions;
using FightClub.Domain.ValueObjects;

namespace FightClub.Domain.Entities;

public class FightRound : Entity
{
    private readonly List<RoundEvent> _events = new();

    public int Number { get; private set; }

    public int ScoreA { get; private set; }
    public int ScoreB { get; private set; }
    
    public bool IsFinished { get; private set; }
    public IReadOnlyCollection<RoundEvent> Events
        => _events.AsReadOnly();

    internal FightRound(int number)
    {
        Number = number;
    }
    internal void AddEvent(RoundEvent roundEvent)
    {
        _events.Add(roundEvent);
    }

    internal void SetScore(RoundScore score)
    {
        if (IsFinished)
            throw new DomainException("Round already finished");

        ScoreA = score.ScoreA;
        ScoreB = score.ScoreB;

        IsFinished = true;
    }
}