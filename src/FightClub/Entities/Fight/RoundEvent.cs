namespace FightClub.Entities.Fight;

public class RoundEvent
{
    public Guid Id { get; set; }

    public Guid FightRoundId { get; set; }
    public FightRound FightRound { get; set; } = null!;

    public EventType Type { get; set; }

    public Guid? SourceBoxerId { get; set; }

    public int Impact { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
