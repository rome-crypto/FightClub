using FightClub.Domain.Enums;
using FightClub.Domain.Exceptions;
using FightClub.Domain.Policies;
using FightClub.Domain.Common;

namespace FightClub.Domain.Entities;

public class Fight : AggregateRoot
{
    private readonly List<FightRound> _rounds = new();

    public Guid BoxerAId { get; private set; }
    public Guid BoxerBId { get; private set; }

    public Guid? WinnerId { get; private set; }
    public FightStatus Status { get; private set; }
    public FightEndType? EndType { get; private set; }

    public int PlannedRounds { get; private set; }

    public int ActualRounds => _rounds.Count;
    public int TotalScoreA =>
        _rounds.Sum(r => r.ScoreA);
    public int TotalScoreB =>
        _rounds.Sum(r => r.ScoreB);
    public IReadOnlyCollection<FightRound> Rounds 
        => _rounds.AsReadOnly();

    private Fight() { }

    public Fight(Guid boxerAId, Guid boxerBId, int plannedRounds)
    {
        if (boxerAId == boxerBId)
            throw new DomainException("Boxer cannot fight himself");
        if (plannedRounds < 1 || plannedRounds > 12)
            throw new DomainException("Rounds must be between 1 and 12");

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
        if (_rounds.LastOrDefault() is { IsFinished: false })
            throw new DomainException("Finish current round first.");

        if (Status != FightStatus.InProgress)
            throw new DomainException("Fight not active");

        if (ActualRounds >= PlannedRounds)
            throw new DomainException("Maximum rounds reached");

        _rounds.Add(
            new FightRound(ActualRounds + 1)
        );
    }

    internal void Finish(
        Guid? winnerId,
        FightEndType endType)
    {
        if (winnerId.HasValue &&
            winnerId != BoxerAId &&
            winnerId != BoxerBId)
        {
            throw new DomainException("Winner must participate in fight.");
        }

        if (Status == FightStatus.Finished)
            throw new DomainException("Fight already finished");

        WinnerId = winnerId;
        EndType = endType;
        Status = FightStatus.Finished;
    }

    public void RegisterEvent(RoundEvent roundEvent)
    {
        if (Status != FightStatus.InProgress)
            throw new DomainException("Fight not active");

        if (ActualRounds == 0)
            throw new DomainException("No active round");

        if (roundEvent.BoxerId != BoxerAId &&
            roundEvent.BoxerId != BoxerBId)
        {
            throw new DomainException("Boxer is not a participant of this fight.");
        }

        _rounds.Last().AddEvent(roundEvent);
    }

    public void FinishRound(
        int scoreA,
        int scoreB,
        IFightEndingPolicy policy)
    {
        if (Status != FightStatus.InProgress)
            throw new DomainException("Fight not active");

        if (ActualRounds == 0)
            throw new DomainException("No active round");


        _rounds.Last()
            .SetScore(scoreA, scoreB);


        if (policy.TryFinish(
            this,
            out var winnerId,
            out var endType))
        {
            Finish(
                winnerId,
                endType);
        }
    }
}