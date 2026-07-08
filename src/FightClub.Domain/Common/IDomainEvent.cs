namespace FightClub.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}