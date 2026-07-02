namespace FightClub.Entities.Fight;

public class Fight
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BoxerAId { get; set; }
    public Boxer BoxerA { get; set; } = null!;

    public Guid BoxerBId { get; set; }
    public Boxer BoxerB { get; set; } = null!;

    public FightStatus Status { get; set; } = FightStatus.Scheduled;

    public Guid? WinnerId { get; set; }

    public int PlannedRounds { get; set; }

    public List<FightRound> Rounds { get; set; } = new();
}
