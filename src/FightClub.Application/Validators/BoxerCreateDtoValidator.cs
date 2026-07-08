using FluentValidation;
using FightClub.DTOs.Boxers;
namespace FightClub.Validators;

public class BoxerCreateDtoValidator : AbstractValidator<BoxerCreateDto>
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
        
        RuleFor(x => x.Age)
            .GreaterThan(0).WithMessage("Age must be a positive number.")
            .LessThanOrEqualTo(120).WithMessage("Age cannot exceed 120.");

        RuleFor(x => x.WeightCategory)
            .NotEmpty().WithMessage("Weight category is required.")
            .MinimumLength(2).WithMessage("Weight category must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Weight category cannot exceed 50 characters.");
    }

}
