using FightClub.Application.DTOs.Trainers;
using FluentValidation;

namespace FightClub.Application.Validators.Trainers;

public sealed class TrainerCreateDtoValidator
    : AbstractValidator<TrainerCreateDto>
{
    public TrainerCreateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
                .WithMessage("First name is required.")
            .Length(2, 50)
                .WithMessage("First name must be between 2 and 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
                .WithMessage("Last name is required.")
            .Length(2, 50)
                .WithMessage("Last name must be between 2 and 50 characters.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today.AddYears(-18))
                .WithMessage("Trainer must be at least 18 years old.")
            .GreaterThan(DateTime.Today.AddYears(-100))
                .WithMessage("Trainer age cannot exceed 100 years.");
    }
}