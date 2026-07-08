using FightClub.Application.DTOs.Boxers;
using FluentValidation;

namespace FightClub.Application.Validators.Boxer;

public sealed class BoxerCreateDtoValidator
    : AbstractValidator<BoxerCreateDto>
{
    public BoxerCreateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today.AddYears(-18))
            .WithMessage("Boxer must be at least 18 years old.")
            .GreaterThan(DateTime.Today.AddYears(-80))
            .WithMessage("Boxer age cannot exceed 80 years.");

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(30)
            .WithMessage("Weight must be at least 30 kg.")
            .LessThanOrEqualTo(200)
            .WithMessage("Weight cannot exceed 200 kg.");
    }
}