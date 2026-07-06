using FightClub.Domain.Exceptions;
using FightClub.Domain.ValueObjects;

namespace FightClub.Domain.Entities;

public class Boxer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int Age { get; private set; }
    public WeightCategory WeightCategory { get; private set; }
    public Guid? TrainerId { get; private set; }

    // Статистика
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Draws { get; private set; }
    public double WinRate => Wins + Losses == 0
        ? 0
        : Math.Round((double)Wins / (Wins + Losses) * 100, 2);

    // EF Core нужен пустой конструктор
    private Boxer() { }

    // Основной конструктор для создания
    public Boxer(string firstName, string lastName, int age, WeightCategory weightCategory, Guid? trainerId = null)
    {
        SetName(firstName, lastName);
        SetAge(age);
        WeightCategory = weightCategory ?? throw new DomainException("Weight category is required");
        TrainerId = trainerId;
        Id = Guid.NewGuid();
        Wins = 0;
        Losses = 0;
        Draws = 0;
    }

    // Бизнес-методы
    public void UpdateProfile(string? firstName, string? lastName, int? age)
    {
        if (firstName is not null || lastName is not null)
            SetName(firstName ?? FirstName, lastName ?? LastName);

        if (age.HasValue)
            SetAge(age.Value);
    }

    public void AssignTrainer(Guid trainerId) => TrainerId = trainerId;
    public void RemoveTrainer() => TrainerId = null;

    public void UpdateStatistics(FightResult result)
    {
        switch (result)
        {
            case FightResult.Win: Wins++; break;
            case FightResult.Loss: Losses++; break;
            case FightResult.Draw: Draws++; break;
        }
    }

    private void SetName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");
        if (firstName.Length > 50 || lastName.Length > 50)
            throw new DomainException("Name cannot exceed 50 characters");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    private void SetAge(int age)
    {
        if (age < 18 || age > 60)
            throw new DomainException("Age must be between 18 and 60");
        Age = age;
    }
}