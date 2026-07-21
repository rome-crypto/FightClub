using FightClub.Domain.Common;
using FightClub.Domain.Enums;
using FightClub.Domain.Exceptions;

namespace FightClub.Domain.Entities;

public class RoundEvent : Entity
{
    public RoundEventType Type { get; }
    public Guid BoxerId { get; }
    public DateTime OccurredAt { get; }

    public RoundEvent(
        RoundEventType type,
        Guid boxerId)
    {
        if (boxerId == Guid.Empty)
        {
            throw new DomainException("BoxerId cannot be empty.");
        }

        Id = Guid.NewGuid();
        Type = type;
        BoxerId = boxerId;
        OccurredAt = DateTime.UtcNow;
    }
}
