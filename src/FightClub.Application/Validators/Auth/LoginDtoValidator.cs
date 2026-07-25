using FightClub.Application.DTOs.Auth;
using FluentValidation;

namespace FightClub.Application.Validators.Auth;

public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        // Email валидация
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

        // Password валидация
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
