using FightClub.Domain.Enums;

namespace FightClub.Domain.Entities;

public class FightRound
{
    public Guid Id { get; private set; }

    public Fight Fight { get; private set; }

    public int Number { get; private set; }

    public int ScoreA { get; private set; }
    public int ScoreB { get; private set; }

    public IReadOnlyList<RoundEvent> Events { get; set; }

    public FightRound(int number, int scoreA, int scoreB)
    {
        if ()
    }

}
