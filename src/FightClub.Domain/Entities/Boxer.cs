using FightClub.Domain.Exceptions;
using FightClub.Domain.ValueObjects;
using FightClub.Domain.Common;
using FightClub.Domain.Enums;

namespace FightClub.Domain.Entities;

public class Boxer : AggregateRoot
{
    // Профиль
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth { get; private set; }
    public int Age => CalculateAge(DateOfBirth);
    public int Weight { get; private set; }
    public WeightCategory WeightCategory => WeightCategory.FromWeight(Weight);
    public Guid? TrainerId { get; private set; }

    // Статистика
    public BoxerStatistics Statistics { get; private set; } = new BoxerStatistics();
    public BoxerRanking Ranking { get; private set; } = new BoxerRanking();

    // EF Core нужен пустой конструктор
    private Boxer() : base()
    {
    }

    // Основной конструктор для создания
    public Boxer(string firstName, string lastName, DateTime birthDate, int weight, Guid? trainerId = null)
    {
        SetName(firstName, lastName);
        SetBirthDate(birthDate);
        SetWeight(weight);
        AssignTrainer(trainerId);
    }

    // Бизнес-методы
    public void ChangeBirthDate(DateTime? birthDate)
    {
        if (birthDate.HasValue)
        {
            SetBirthDate(birthDate.Value);
        }
    }

    public void Rename(string? firstName, string? lastName)
    {
        if (firstName == null || lastName == null)
        {
            return;
        }

        SetName(firstName, lastName);
    }

    public void ChangeWeight(int? weight)
    {
        if (weight.HasValue)
        {
            SetWeight(weight.Value);
        } 
    }

    private static int CalculateAge(DateTime birthDate)
    {
        DateTime today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        
        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    internal void ApplyFightResult(
        FightResult result,
        FightEndType endType,
        int newElo)
    {
        switch (result)
        {
            case FightResult.Win:
                Statistics.RegisterWin(
                    knockout: endType == FightEndType.Knockout,
                    technicalKnockout: endType == FightEndType.TechnicalKnockout);
                break;

            case FightResult.Loss:
                Statistics.RegisterLoss(
                    knockout: endType == FightEndType.Knockout,
                    technicalKnockout: endType == FightEndType.TechnicalKnockout);
                break;

            case FightResult.Draw:
                Statistics.RegisterDraw();
                break;
            
            default:
                throw new DomainException("Unknown fight result");
        }

        Ranking.UpdateElo(newElo);
    }

    public void AssignTrainer(Guid? trainerId) 
    {
        if (trainerId == Guid.Empty)
        {
            throw new DomainException("Trainer ID is invalid");
        }
        
        if (TrainerId == trainerId)
        {
            return;
        }

        TrainerId = trainerId;
    }

    public void RemoveTrainer()
    {
        if (TrainerId is null)
        {

            return;
        }

        TrainerId = null;
    }

    private void SetName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new DomainException("First name is required");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new DomainException("Last name is required");
        }

        if (firstName.Length > 50)
        {
            throw new DomainException("First name cannot exceed 50 characters");
        }
        if (lastName.Length > 50)
        {
            throw new DomainException("Last name cannot exceed 50 characters");
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    private void SetWeight(int weight)
    {
        if (weight < 30 || weight > 200)
        {
            throw new DomainException("Invalid weight.");
        }

        _ = WeightCategory.FromWeight(weight);

        Weight = weight;
    }

    private void SetBirthDate(DateTime birthDate)
    {
        var age = CalculateAge(birthDate);
        if (age < 18 || age > 80)
        {
            throw new DomainException("Age must be between 18 and 80");
        }
        DateOfBirth = birthDate;
    }
}

