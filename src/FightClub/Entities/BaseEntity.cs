namespace FightClub.Entities;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}
