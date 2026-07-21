using FightClub.Domain.Entities;
using FightClub.Domain.Enums;
using FightClub.Domain.Exceptions;
using FightClub.Domain.Policies;
using FightClub.Domain.ValueObjects;

namespace FightClub.Domain.Tests.Entities;

[TestClass]
public class FightTests
{
    [TestMethod]
    public void ConstructorShouldCreateCreatedFightWhenDateIsNull()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();

        var fight = new Fight(boxerA, boxerB);

        Assert.AreEqual(FightStatus.Created, fight.Status);
        Assert.IsNull(fight.FightDate);
    }

    [TestMethod]
    public void ConstructorShouldCreateScheduledFightWhenDateProvided()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        DateTime date = DateTime.UtcNow.AddDays(7);

        var fight = new Fight(boxerA, boxerB, date);

        Assert.AreEqual(FightStatus.Scheduled, fight.Status);
        Assert.AreEqual(date, fight.FightDate);
    }

    [TestMethod]
    public void ConstructorShouldThrowWhenBoxersAreEqual()
    {
        var boxer = Guid.NewGuid();

        Assert.ThrowsExactly<DomainException>(
            () => new Fight(boxer, boxer));
    }

    [TestMethod]
    public void RescheduleShouldThrowWhenDateInPast()
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
    public void RescheduleShouldUpdateFightDate()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        DateTime newDate = DateTime.UtcNow.AddDays(10);

        fight.Reschedule(newDate);

        Assert.AreEqual(newDate, fight.FightDate);
        Assert.AreEqual(FightStatus.Scheduled, fight.Status);
    }

    [TestMethod]
    public void StartShouldChangeStatusToInProgress()
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
    public void StartShouldThrowWhenFightIsNotScheduled()
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
    public void StartShouldThrowWhenFightAlreadyStarted()
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
    public void CancelShouldChangeStatusToCancelled()
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
    public void CompleteShouldSetStatusToFinishedAndWinner()
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
    public void CompleteShouldThrowWhenOutcomeNotFinished()
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
    public void CompleteShouldThrowWhenFightNotStarted()
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
    public void StartRoundShouldCreateFirstRound()
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
    public void StartRoundShouldThrowWhenFightNotStarted()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1));

        Assert.ThrowsExactly<DomainException>(
            () => fight.StartRound());
    }

    [TestMethod]
    public void StartRoundShouldThrowWhenPreviousRoundNotFinished()
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
    public void RegisterEventShouldThrowWhenNoActiveRound()
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
    public void RegisterEventShouldThrowWhenBoxerNotParticipant()
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
    public void RegisterEventShouldAddEvent()
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
    public void ConstructorShouldThrowWhenPlannedRoundsLessThanOne()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();

        var plannedRounds = 0;

        Assert.ThrowsExactly<DomainException>(
            () => new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1), plannedRounds));
    }


    [TestMethod]
    public void ConstructorShouldThrowWhenPlannedRoundsGreaterThanMaxPlannedRounds()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();

        var plannedRounds = Fight.MaxPlannedRounds + 1;

        Assert.ThrowsExactly<DomainException>(
            () => new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1), plannedRounds));
    }

    [TestMethod]
    public void RescheduleShouldThrowWhenFightIsInProgress()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));

        fight.Start();

        Assert.ThrowsExactly<DomainException>(
            () => fight.Reschedule(DateTime.UtcNow.AddDays(10)));
    }

    [TestMethod]
    public void RescheduleShouldThrowWhenFightIsCancelled()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));

        fight.Cancel();

        Assert.ThrowsExactly<DomainException>(
            () => fight.Reschedule(DateTime.UtcNow.AddDays(10)));
    }

    [TestMethod]
    public void StartShouldThrowWhenFightIsCancelled()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1));

        fight.Cancel();

        Assert.ThrowsExactly<DomainException>(
            () => fight.Start());
    }

    [TestMethod]
    public void CancelShouldThrowWhenFightIsFinished()
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
    public void StartRoundShouldCreateUnfinishedRound()
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
    public void StartRoundShouldThrowWhenMaximumRoundsReached()
    {
        var fight = new Fight(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1),
            2);

        fight.Start();

        fight.StartRound();
        fight.EndCurrentRound(
            new RoundScore(10, 10),
            new BoxingFightEndingPolicy());

        fight.StartRound();

        fight.EndCurrentRound(
            new RoundScore(10, 9),
            new BoxingFightEndingPolicy());

        Assert.ThrowsExactly<DomainException>(
            () => fight.StartRound());
    }


    [TestMethod]
    public void RegisterEventShouldThrowWhenRoundIsFinished()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1), 3);
        var roundEvent = new RoundEvent(RoundEventType.Punch, boxerA);

        fight.Start();
        fight.StartRound();
        fight.EndCurrentRound(new RoundScore(10, 9), new BoxingFightEndingPolicy());


        Assert.ThrowsExactly<DomainException>(
            () => fight.RegisterEvent(roundEvent));
    }

    [TestMethod]
    public void EndCurrentRoundShouldFinishCurrentRound()
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
    public void EndCurrentRoundShouldThrowWhenNoActiveRound()
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
    public void EndCurrentRoundShouldContinueFightWhenNotAllRoundsFinished()
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
    public void EndCurrentRoundShouldFinishFightWhenLastRoundFinished()
    {
        var boxerA = Guid.NewGuid();
        var boxerB = Guid.NewGuid();
        var fight = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(1), 1);
        var roundScore = new RoundScore(10, 9);
        var policy = new BoxingFightEndingPolicy();

        fight.Start();
        fight.StartRound();
        fight.EndCurrentRound(roundScore, policy);

        Assert.AreEqual(FightStatus.Finished, fight.Status);
        Assert.IsTrue(fight.Rounds.Last().IsFinished);
    }

    [TestMethod]
    public void CompleteShouldThrowWhenFightAlreadyFinished()
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
    public void IsAllowedChangesCheck()
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

        var fight2 = new Fight(boxerA, boxerB, DateTime.UtcNow.AddDays(10));
        fight2.Cancel();
        var state5 = fight2.IsAllowedChanges;

        Assert.IsTrue(state1);
        Assert.IsTrue(state2);
        Assert.IsFalse(state3);
        Assert.IsFalse(state4);
        Assert.IsFalse(state5);
    }
}
