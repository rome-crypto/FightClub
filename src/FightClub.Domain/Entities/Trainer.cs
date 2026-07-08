using FightClub.Domain.Common;
using FightClub.Domain.Exceptions;

namespace FightClub.Domain.Entities;

public class Trainer : Entity
{
    public Guid Id { get; private set; }

    // Профиль
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth { get; private set; }
    public int Age => CalculateAge(DateOfBirth);


    private Trainer() { }

    public Trainer(string firstName, string lastName, DateTime birthDate)
    {
        SetName(firstName, lastName);
        SetBirthDate(birthDate);
        Id = Guid.NewGuid();
    }

    public void UpdateInfo(string? firstName, string? lastName, DateTime? birthDate)
    {
        if (firstName is not null || lastName is not null)
            SetName(firstName ?? FirstName, lastName ?? LastName);

        if (birthDate.HasValue)
            SetBirthDate(birthDate.Value);
    }

    private void SetName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");
        if (firstName.Length > 100 || lastName.Length > 100)
            throw new DomainException("Name cannot exceed 100 characters");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    private void SetBirthDate(DateTime birthDate)
    {
        var age = CalculateAge(birthDate);
        if (age < 18 || age > 100)
            throw new DomainException("Age must be between 18 and 100");
        DateOfBirth = birthDate;
    }

    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
}