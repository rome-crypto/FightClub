using FightClub.Domain.Enums;

namespace FightClub.Domain.Entities;

public class RoundEvent
{
    public Guid Id { get; set; }

    public Guid FightRoundId { get; set; }
    public FightRound FightRound { get; set; } = null!;

    public EventType Type { get; private set; }

    public Guid? SourceBoxerId { get; set; }

    public int Impact { get; private set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
