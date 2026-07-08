using FightClub.Application.DTOs.Trainers;
using FluentValidation;

namespace FightClub.Application.Validators.Trainer;

public sealed class TrainerUpdateDtoValidator
    : AbstractValidator<TrainerUpdateDto>
{
    public TrainerUpdateDtoValidator()
    {
        When(x => x.FirstName is not null, () =>
        {
            RuleFor(x => x.FirstName!)
                .Length(2, 50)
                .WithMessage("First name must be between 2 and 50 characters.");
        });

        When(x => x.LastName is not null, () =>
        {
            RuleFor(x => x.LastName!)
                .Length(2, 50)
                .WithMessage("Last name must be between 2 and 50 characters.");
        });

        When(x => x.BirthDate.HasValue, () =>
        {
            RuleFor(x => x.BirthDate!.Value)
                .LessThan(DateTime.Today.AddYears(-18))
                    .WithMessage("Trainer must be at least 18 years old.")
                .GreaterThan(DateTime.Today.AddYears(-100))
                    .WithMessage("Trainer age cannot exceed 100 years.");
        });
    }
}