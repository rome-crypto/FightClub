namespace FightClub.Domain.Common;

public interface IDomainEvent
{
    public DateTime OccurredAt { get; }
}
