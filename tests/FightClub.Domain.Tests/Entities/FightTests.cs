using FightClub.Domain.Entities;
using FightClub.Domain.Exceptions;
using FightClub.Domain.Policies;
using FightClub.Domain.Services;
using FightClub.Domain.Tests.Policies;
using FightClub.Domain.ValueObjects;

namespace FightClub.Domain.Tests.Entities;

[TestClass]
public class FightTests
{
    [TestMethod]
    public void Constructor_Should_Create_Created_Fight_When_Date_Is_Null()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();

        var fight = new Fight(boxerA, boxerB);

        Assert.AreEqual(FightStatus.Created, fight.Status);
        Assert.IsNull(fight.FightDate);
    }

    [TestMethod]
    public void Constructor_Should_Create_Scheduled_Fight_When_Date_Provided()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var date = DateTime.UtcNow.AddDays(7);

        var fight = new Fight(boxerA, boxerB, date);

        Assert.AreEqual(FightStatus.Scheduled, fight.Status);
        Assert.AreEqual(date, fight.FightDate);
    }

    [TestMethod]
    public void Constructor_Should_Throw_When_Boxers_Are_Equal()
    {
        var boxer = Guid.NewGuid();

        Assert.ThrowsExactly<DomainException>(
            () => new Fight(boxer, boxer));
    }

    [TestMethod]
    public void Reschedule_Should_Throw_When_Date_In_Past()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        Assert.ThrowsExactly<DomainException>(
            () => fight.Reschedule(DateTime.UtcNow.AddDays(-1))
        );
    }

    [TestMethod]
    public void Reschedule_Should_Update_Fight_Date()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        var newDate = DateTime.UtcNow.AddDays(10);

        fight.Reschedule(newDate);

        Assert.AreEqual(newDate, fight.FightDate);
        Assert.AreEqual(FightStatus.Scheduled, fight.Status);
    }

    [TestMethod]
    public void Start_Should_Change_Status_To_InProgress()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(5)
        );

        fight.Start();

        Assert.AreEqual(
            FightStatus.InProgress,
            fight.Status
        );
    }

    [TestMethod]
    public void Start_Should_Throw_When_Fight_Is_Not_Scheduled()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        Assert.ThrowsExactly<DomainException>(
            () => fight.Start()
        );
    }

    [TestMethod]
    public void Start_Should_Throw_When_Fight_Already_Started()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(5)
        );

        fight.Start();

        Assert.ThrowsExactly<DomainException>(
            () => fight.Start()
        );
    }

    [TestMethod]
    public void Cancel_Should_Change_Status_To_Cancelled()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(5)
        );

        fight.Cancel();

        Assert.AreEqual(
            FightStatus.Cancelled,
            fight.Status
        );
    }

    [TestMethod]
    public void Complete_Should_Set_Status_To_Finished_And_Winner()
    {
        var boxerAId = Guid.NewGuid();
        var boxerBId = Guid.NewGuid();

        var fight = new Fight(
            boxerAId,
            boxerBId,
            DateTime.UtcNow.AddDays(1));

        fight.Start();

        var outcome = FightOutcome.Finish(
            boxerAId,
            FightEndType.Knockout);

        fight.Complete(outcome);

        Assert.AreEqual(FightStatus.Finished, fight.Status);
        Assert.AreEqual(boxerAId, fight.WinnerId);

        Assert.AreEqual(FightEndType.Knockout, fight.EndType);
    }
    
    [TestMethod]
    public void Complete_Should_Throw_When_Outcome_Not_Finished()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1));

        fight.Start();

        var outcome = FightOutcome.Continue();

        Assert.ThrowsExactly<DomainException>(
            () => fight.Complete(outcome));
    }

    [TestMethod]
    public void Complete_Should_Throw_When_Fight_Not_Started()
    {
        var outcome = FightOutcome.Finish(
            Guid.NewGuid(),
            FightEndType.Decision);

        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1));

        Assert.ThrowsExactly<DomainException>(
            () => fight.Complete(outcome));
    }

    [TestMethod]
    public void StartRound_Should_Create_First_Round()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1));

        fight.Start();

        fight.StartRound();

        Assert.AreEqual(1, fight.ActualRounds);
        Assert.HasCount(1, fight.Rounds);
    }

    [TestMethod]
    public void StartRound_Should_Throw_When_Fight_Not_Started()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1));

        Assert.ThrowsExactly<DomainException>(
            () => fight.StartRound());
    }

    [TestMethod]
    public void StartRound_Should_Throw_When_Previous_Round_Not_Finished()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1));

        fight.Start();

        fight.StartRound();

        Assert.ThrowsExactly<DomainException>(
            () => fight.StartRound());
    }

    [TestMethod]
    public void RegisterEvent_Should_Throw_When_No_Active_Round()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();

        var fight = new Fight(
            boxerA,
            boxerB,
            DateTime.UtcNow.AddDays(1));

        fight.Start();

        var roundEvent = new RoundEvent(RoundEventType.Punch, boxerA);

        Assert.ThrowsExactly<DomainException>(
            () => fight.RegisterEvent(roundEvent));
    }

    [TestMethod]
    public void RegisterEvent_Should_Throw_When_Boxer_Not_Participant()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();

        var stranger = Guid.NewGuid();

        var fight = new Fight(
            boxerA,
            boxerB,
            DateTime.UtcNow.AddDays(1));

        fight.Start();

        fight.StartRound();

        var roundEvent = new RoundEvent(RoundEventType.Punch, stranger);

        Assert.ThrowsExactly<DomainException>(
            () => fight.RegisterEvent(roundEvent));
    }

    [TestMethod]
    public void RegisterEvent_Should_Add_Event()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();

        var fight = new Fight(
            boxerA,
            boxerB,
            DateTime.UtcNow.AddDays(1));

        fight.Start();

        fight.StartRound();

        var roundEvent = new RoundEvent(RoundEventType.Punch, boxerA);

        fight.RegisterEvent(roundEvent);

        Assert.HasCount(1, fight.Rounds.First().Events);
        Assert.IsTrue(fight.Rounds.Last().Events.Contains(roundEvent));
    }

    [TestMethod]
    public void Constructor_Should_Throw_When_PlannedRounds_Less_Than_One()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();

        var plannedRounds = 0;

        Assert.ThrowsExactly<DomainException>(
            () => new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1), plannedRounds));
    }


    [TestMethod]
    public void Constructor_Should_Throw_When_PlannedRounds_Greater_Than_MaxPlannedRounds()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();

        var plannedRounds = Fight.MaxPlannedRounds + 1;

        Assert.ThrowsExactly<DomainException>(
            () => new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1), plannedRounds));
    }

    [TestMethod]
    public void Reschedule_Should_Throw_When_Fight_Is_InProgress()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));

        fight.Start();

        Assert.ThrowsExactly<DomainException>(
            () => fight.Reschedule(DateTime.UtcNow.AddDays(10)));
    }

    [TestMethod]
    public void Reschedule_Should_Throw_When_Fight_Is_Cancelled()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));

        fight.Cancel();

        Assert.ThrowsExactly<DomainException>(
            () => fight.Reschedule(DateTime.UtcNow.AddDays(10)));
    }

    [TestMethod]
    public void Start_Should_Throw_When_Fight_Is_Cancelled()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));

        fight.Cancel();

        Assert.ThrowsExactly<DomainException>(
            () => fight.Start());
    }

    [TestMethod]
    public void Cancel_Should_Throw_When_Fight_Is_Finished()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));

        fight.Start();
        fight.Complete(FightOutcome.Finish(boxerA, FightEndType.Decision));

        Assert.ThrowsExactly<DomainException>(
            () => fight.Cancel());
    }

    [TestMethod]
    public void Cancel_Should_Throw_When_Fight_Is_Created()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB);

        fight.Start();
        fight.Cancel();

        Assert.AreEqual(FightStatus.Cancelled, fight.Status);
    }

    [TestMethod]
    public void StartRound_Should_Create_Unfinished_Round()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));

        fight.Start();
        fight.StartRound();

        Assert.HasCount(1, fight.Rounds);
        Assert.IsFalse(fight.Rounds.Last().IsFinished);
    }

    [TestMethod]
    public void StartRound_Should_Throw_When_Maximum_Rounds_Reached()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1), 1);

        fight.Start();
        fight.StartRound();
        fight.EndCurrentRound(new RoundScore(10, 10), new BoxingFightEndingPolicy());

        Assert.ThrowsExactly<DomainException>(
            () => fight.StartRound());
    }


    [TestMethod]
    public void RegisterEvent_Should_Throw_When_Round_Is_Finished()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));
        var roundEvent = new RoundEvent(RoundEventType.Punch, boxerA);

        fight.Start();
        fight.StartRound();
        fight.EndCurrentRound(new RoundScore(10, 9), new BoxingFightEndingPolicy());
        

        Assert.ThrowsExactly<DomainException>(
            () => fight.RegisterEvent(roundEvent));
    }

    [TestMethod]
    public void EndCurrentRound_Should_Finish_Current_Round()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));
        var roundScore = new RoundScore(10, 9);
        var policy = new BoxingFightEndingPolicy();

        fight.Start();
        fight.StartRound();
        fight.EndCurrentRound(roundScore, policy);

        Assert.IsTrue(fight.Rounds.Last().IsFinished);
        Assert.AreEqual(roundScore.ScoreA, fight.Rounds.Last().ScoreA);
        Assert.AreEqual(roundScore.ScoreB, fight.Rounds.Last().ScoreB);
    }

    [TestMethod]
    public void EndCurrentRound_Should_Throw_When_No_Active_Round()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));
        var roundScore = new RoundScore(10, 9);
        var policy = new BoxingFightEndingPolicy();

        fight.Start();

        Assert.AreEqual(FightStatus.InProgress, fight.Status);
        Assert.ThrowsExactly<DomainException>(
            () => fight.EndCurrentRound(roundScore, policy));
    }

    [TestMethod]
    public void EndCurrentRound_Should_Continue_Fight_When_Not_All_Rounds_Finished()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1), 3);
        var roundScore = new RoundScore(10, 9);
        var policy = new BoxingFightEndingPolicy();

        fight.Start();
        fight.StartRound();
        fight.EndCurrentRound(roundScore, policy);

        Assert.AreEqual(FightStatus.InProgress, fight.Status);
    }

    [TestMethod]
    public void EndCurrentRound_Should_Finish_Fight_When_Last_Round_Finished()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB,DateTime.UtcNow.AddDays(1), 1);
        var roundScore = new RoundScore(10, 9);
        var policy = new BoxingFightEndingPolicy();

        fight.Start();
        fight.StartRound();
        fight.EndCurrentRound(roundScore, policy);

        Assert.AreEqual(FightStatus.InProgress, fight.Status);
        Assert.IsTrue(fight.Rounds.Last().IsFinished);
    }

    [TestMethod]
    public void Complete_Should_Throw_When_Fight_Already_Finished()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));

        fight.Start();
        fight.Complete(FightOutcome.Finish(boxerA, FightEndType.Decision));

        Assert.ThrowsExactly<DomainException>(
            () => fight.Complete(FightOutcome.Finish(boxerA, FightEndType.Decision)));
    }

    [TestMethod]
    public void IsAllowedChanges_Check()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        
        var fight = new Fight(boxerA, boxerB);
        var state1 = fight.IsAllowedChanges;
        
        fight.Reschedule(DateTime.UtcNow.AddDays(5));
        var state2 = fight.IsAllowedChanges;

        fight.Start();
        var state3 = fight.IsAllowedChanges;

        fight.Complete(FightOutcome.Finish(boxerA, FightEndType.Decision));
        var state4 = fight.IsAllowedChanges;

        var fight2 = new Fight(boxerA, boxerB);
        fight2.Cancel();
        var state5 = fight2.IsAllowedChanges;

        Assert.IsTrue(state1);
        Assert.IsTrue(state2);
        Assert.IsFalse(state3);
        Assert.IsFalse(state4);
        Assert.IsFalse(state5);
    }
}
