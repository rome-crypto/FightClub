using FightClub.Domain.Exceptions;
using FightClub.Domain.ValueObjects;
using FightClub.Domain.Enums;

namespace FightClub.Domain.Entities;

public class Boxer
{
    public Guid Id { get; private set; }

    // Профиль
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth { get; private set; }
    public int Age => CalculateAge(DateOfBirth);
    public int Weight { get; private set; }
    public WeightCategory WeightCategory => WeightCategory.FromWeight(Weight);
    public Guid? TrainerId { get; private set; }

    // Статистика
    public BoxerStats Stats { get; private set; }

    // EF Core нужен пустой конструктор
    private Boxer() { }

    // Основной конструктор для создания
    public Boxer(string firstName, string lastName, DateTime birthDate, int weight, Guid? trainerId = null)
    {
        SetName(firstName, lastName);
        SetBirthDate(birthDate);
        Weight = weight;
        TrainerId = trainerId;
        Id = Guid.NewGuid();
        Stats = new BoxerStats(0,0,0);
    }

    // Бизнес-методы
    public void UpdateInfo(string? firstName, string? lastName, DateTime? birthDate)
    {
        if (firstName is not null || lastName is not null)
            SetName(firstName ?? FirstName, lastName ?? LastName);

        if (birthDate.HasValue)
            SetBirthDate(birthDate.Value);
    }

    public static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        
        if (birthDate.Date > today.AddYears(-age)) 
            age--;

        return age;
    }

    public void AssignTrainer(Guid trainerId) 
    {
        if (trainerId == Guid.Empty)
            throw new DomainException("Trainer ID is invalid");

        TrainerId = trainerId;
    } 
    public void RemoveTrainer() => TrainerId = null;

    private void SetName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");
        if (firstName.Length > 50)
            throw new DomainException("First name cannot exceed 50 characters");
        if (lastName.Length > 50)
            throw new DomainException("Last name cannot exceed 50 characters");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    private void SetBirthDate(DateTime birthDate)
    {
        var age = CalculateAge(birthDate);
        if (age < 18 || age > 80)
            throw new DomainException("Age must be between 18 and 80");
        DateOfBirth = birthDate;
    }
}