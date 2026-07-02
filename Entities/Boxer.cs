namespace FightClub.Entities;

public class Boxer
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string FirstName { get;  set; } = string.Empty;
    public string LastName { get;  set; } = string.Empty;

    private int _age;
    public int Age
    {
        get => _age;
        set => _age = value > 0 ? value : 0;
    }

    public string WeightCategory { get; set; } = string.Empty;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Guid TrainerId { get; set; }
    public Trainer Trainer { get; set; } = null!;

    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public double WinRate =>
        Wins + Losses == 0
            ? 0
            : Math.Round((double)Wins / (Wins + Losses) * 100, 2);


    public List<Fight> FightsAsA { get; set; } = new();
    public List<Fight> FightsAsB { get; set; } = new();


    public Boxer() {}

    public Boxer(string firstName, string lastName, int age, string weightCategory, Trainer trainer)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        WeightCategory = weightCategory;
        TrainerId = trainer.Id;
        Trainer = trainer;
    }
}
