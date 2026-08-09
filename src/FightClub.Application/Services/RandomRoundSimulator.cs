using FightClub.Application.Interfaces;
using FightClub.Domain.ValueObjects;


namespace FightClub.Application.Services;


public sealed class RandomRoundSimulator
    : IRoundSimulator
{

    private readonly Random _random =
        Random.Shared;

    public RoundScore Simulate(
        Guid boxerAId,
        Guid boxerBId)
    {

        return new RoundScore(
            GenerateScore(),
            GenerateScore());
    }

    private int GenerateScore()
    {
        return _random.Next(7, 12);
    }
}
