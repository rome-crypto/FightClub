namespace FightClub.Models;

public class Boxer
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string FirstName { get;  set; } 
    public string LastName { get;  set; }

    private int _age;
    public int Age
    {
        get => _age;
        set => _age = value > 0 ? value : 0;
    }

    public string WeightCategory { get; set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Boxer() {}

    public Boxer(string firstName, string lastName, int age, string weightCategory)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        WeightCategory = weightCategory;
    }
}
