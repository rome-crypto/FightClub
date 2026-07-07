using FightClub.Domain.Enums;
using FightClub.Domain.Exceptions;

namespace FightClub.Domain.Entities;

public class Fight
{
    private readonly List<FightRound> _rounds = new();

    public Guid Id { get; private set; }

    public Guid BoxerAId { get; private set; }
    public Guid BoxerBId { get; private set; }

    public Guid? WinnerId { get; private set; }

    public FightStatus Status { get; private set; }
    public FightEndType? EndType {  get; private set; }
    public int PlannedRounds { get; private set; }
    public int ActualRounds { get; private set; }
    
    public IReadOnlyCollection<FightRound> Rounds => _rounds;

    private Fight() { }

    public Fight(Guid boxerAId, Guid boxerBId, int plannedRounds)
    {
        if (boxerAId == boxerBId)
            throw new DomainException("Boxer cannot fight himself");
        if (plannedRounds < 1 || plannedRounds > 12)
            throw new DomainException("Rounds must be between 1 and 12");

        Id = Guid.NewGuid();
        BoxerAId = boxerAId;
        BoxerBId = boxerBId;
        PlannedRounds = plannedRounds;
        Status = FightStatus.Scheduled;
    }

    public void Start()
    {
        if (Status != FightStatus.Scheduled)
            throw new DomainException("Fight already started");
        Status = FightStatus.InProgress;
    }

    public void StartRound()
    {
        if (Status != FightStatus.InProgress)
            throw new DomainException("Fight not active");

        if (_rounds.Count >= PlannedRounds)
            throw new DomainException("Maximum rounds reached");

        _rounds.Add(
            new FightRound(_rounds.Count + 1)
        );
    }

    internal void Finish(Guid? winnerId)
    {
        if (Status == FightStatus.Finished)
            return;

        Status = FightStatus.Finished;
        WinnerId = winnerId;
    }

    private bool IsDecisive(FightRound round)
    {
        return Math.Abs(round.ScoreA - round.ScoreB) > 20;
    }

    private Guid? DetermineWinner()
    {
        int totalA = Rounds.Sum(r => r.ScoreA);
        int totalB = Rounds.Sum(r => r.ScoreB);

        if (totalA == totalB) return null;
        return totalA > totalB ? BoxerAId : BoxerBId;
    }
}