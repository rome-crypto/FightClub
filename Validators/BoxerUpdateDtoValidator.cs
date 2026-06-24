using FluentValidation;
using FightClub.DTOs;
namespace FightClub.Validators;

public class BoxerUpdateDtoValidator : AbstractValidator<BoxerUpdateDto>
{
    public BoxerUpdateDtoValidator()
    {
        When(x => x.FirstName is not null, () =>
        {
            RuleFor(x => x.FirstName)
                .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
        });

        When(x => x.LastName is not null, () =>
        {
            RuleFor(x => x.LastName)
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
        });

        When(x => x.Age is not null, () =>
        {
            RuleFor(x => x.Age!.Value)
                .GreaterThan(0).WithMessage("Age must be a positive number.")
                .LessThanOrEqualTo(120).WithMessage("Age cannot exceed 120.");
        });

        When(x => x.WeightCategory is not null, () =>
        {
            RuleFor(x => x.WeightCategory)
                .MinimumLength(2).WithMessage("Weight category must be at least 2 characters long.")
                .MaximumLength(30).WithMessage("Weight category cannot exceed 30 characters.");
        });
    }
}
