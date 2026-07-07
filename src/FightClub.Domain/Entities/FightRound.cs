using FightClub.Domain.Exceptions;

namespace FightClub.Domain.Entities;

public class FightRound
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

    internal void SetScore(int scoreA, int scoreB)
    {
        if (IsFinished)
            throw new DomainException("Round already finished");

        ScoreA = scoreA;
        ScoreB = scoreB;

        IsFinished = true;
    }
}