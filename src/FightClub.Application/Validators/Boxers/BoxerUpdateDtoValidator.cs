using FightClub.Application.DTOs.Boxers;
using FluentValidation;

namespace FightClub.Application.Validators.Boxers;

public sealed class BoxerUpdateDtoValidator
    : AbstractValidator<BoxerUpdateDto>
{
    public BoxerUpdateDtoValidator()
    {
        When(x => x.FirstName is not null, () =>
        {
            RuleFor(x => x.FirstName!)
                .NotEmpty().WithMessage("First name cannot be empty.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
        });

        When(x => x.LastName is not null, () =>
        {
            RuleFor(x => x.LastName!)
                .NotEmpty().WithMessage("Last name cannot be empty.")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
        });

        When(x => x.BirthDate.HasValue, () =>
        {
            RuleFor(x => x.BirthDate!.Value)
                .LessThan(DateTime.Today.AddYears(-18))
                .WithMessage("Boxer must be at least 18 years old.")
                .GreaterThan(DateTime.Today.AddYears(-80))
                .WithMessage("Boxer age cannot exceed 80 years.");
        });

        When(x => x.Weight.HasValue, () =>
        {
            RuleFor(x => x.Weight!.Value)
                .InclusiveBetween(30, 200)
                .WithMessage("Weight must be between 30 and 200 kg.");
        });
    }
}
