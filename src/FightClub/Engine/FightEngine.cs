using FightClub.Entities.Fight;
using System.Runtime.Serialization.Formatters;

namespace FightClub.Engine;

public class FightEngine : IFightEngine
{
    public readonly Random _random = new Random();

    public Fight Execute(Fight fight)
    {
       fight.Status = FightStatus.InProgress;
       
        for (int i = 1; i <= fight.PlannedRounds; i++)
        {
            var round = ProcessRound(fight, i);
            fight.Rounds.Add(round);

            if (CheckEarlyStop(fight, round))
            {
                fight.Status = FightStatus.Finished;
                fight.WinnerId = DetermineWinner(fight);
                return fight;
            }
        }

        fight.Status = FightStatus.Finished;
        fight.WinnerId = DetermineWinner(fight);
        
        return fight;
    }

    private FightRound ProcessRound(Fight fight, int number)
    {
        var round = new FightRound
        {
            Number = number,
        };

        int scoreA = 0;
        int scoreB = 0;

        for (int i = 0; i < 10; i++)
        {
            var eventA = GenerateEvent(fight.BoxerAId);
            var eventB = GenerateEvent(fight.BoxerBId);

            round.Events.Add(eventA);
            round.Events.Add(eventB);

            scoreA += CalculateImpact(eventA);
            scoreB += CalculateImpact(eventB);
        }

        round.ScoreA = scoreA;
        round.ScoreB = scoreB;

        round.Result = scoreA == scoreB 
            ? RoundResult.Draw 
            : scoreA > scoreB 
                ? RoundResult.BoxerA 
                : RoundResult.BoxerB;
        
        return round;
    }

    private RoundEvent GenerateEvent(Guid boxerId)
    {
        var roll = _random.Next(0, 100);

        if (roll < 70)
        {
            return new RoundEvent
            {
                Type = EventType.Punch,
                SourceBoxerId = boxerId,
                Impact = _random.Next(1, 5)
            };
        }

        if (roll < 95)
        {
            return new RoundEvent
            {
                Type = EventType.Block,
                SourceBoxerId = boxerId,
                Impact = _random.Next(0, 2)
            };
        }

        return new RoundEvent
        {
            Type = EventType.Knockdown,
            SourceBoxerId = boxerId,
            Impact = _random.Next(5, 10)
        };
    }

    private int CalculateImpact(RoundEvent roundEvent)
    {
        return roundEvent.Type switch
        {
            EventType.Punch => roundEvent.Impact,
            EventType.Block => -roundEvent.Impact,
            EventType.Knockdown => roundEvent.Impact * 2,
            _ => 0
        };
    }

    private bool CheckEarlyStop(Fight fight, FightRound round)
    {
        return Math.Abs(round.ScoreA - round.ScoreB) > 20;
    }

    private Guid? DetermineWinner(Fight fight)
    {
        int totalScoreA = 0;
        int totalScoreB = 0;

        foreach (var round in fight.Rounds)
        {
            totalScoreA += round.ScoreA;
            totalScoreB += round.ScoreB;
        }

        if (totalScoreA == totalScoreB)
            return null;

        return totalScoreA > totalScoreB 
            ? fight.BoxerAId 
            : fight.BoxerBId;
    }
}
