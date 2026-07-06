namespace FightClub.Entities.Fight;

public class FightRound
{
    public Guid Id { get; set; }

    public Guid FightId { get; set; }
    public Fight Fight { get; set; } = null!;

    public int Number { get; set; }

    public int ScoreA { get; set; }
    public int ScoreB { get; set; }

    public RoundResult Result { get; set; }

    public List<RoundEvent> Events { get; set; } = new();

}
