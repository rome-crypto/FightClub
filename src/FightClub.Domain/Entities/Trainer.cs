using FightClub.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace FightClub.Domain.Entities;

public class Trainer
{
    public Guid Id { get; private set; }
    [Required]
    public string FirstName { get; private set; } = string.Empty;
    [Required]
    public string LastName { get; private set; } = string.Empty;
    [Required]
    public int Age { get; private set; }

    private Trainer() { }

    public Trainer(string firstName, string lastName, int age)
    {
        SetName(firstName, lastName);
        SetAge(age);
        Id = Guid.NewGuid();
    }

    public void Update(string? firstName, string? lastName, int? age)
    {
        if (firstName is not null || lastName is not null)
            SetName(firstName ?? FirstName, lastName ?? LastName);

        if (age.HasValue)
            SetAge(age.Value);
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

    private void SetAge(int age)
    {
        if (age < 18 || age > 80)
            throw new DomainException("Age must be between 18 and 80");
        Age = age;
    }
}