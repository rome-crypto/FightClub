namespace FightClub.Entities;

public class Trainer
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public int Age { get; set; }

    public ICollection<Boxer> Boxers { get; set; } = new List<Boxer>();
}
