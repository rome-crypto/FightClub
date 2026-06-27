namespace FightClub.Entities;

public class Trainer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Boxer> Boxers { get; set; } = new();
}
