using FightClub.Application.DTOs.Auth;
using FluentValidation;

namespace FightClub.Application.Validators.Auth;

public sealed class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        // Username валидация
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters")
            // Почему Matches: username должен быть буквы, цифры, подчеркивание
            // Нельзя спецсимволы типа <script> для защиты от XSS
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers and underscores");

        // Email валидация
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

        // Password валидация
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
            // Почему Must: требуем сложный пароль (буква + цифра минимум)
            // Это базовая защита от weak passwords типа "123456"
            .Must(password => password.Any(char.IsDigit))
                .WithMessage("Password must contain at least one digit")
            .Must(password => password.Any(char.IsLetter))
                .WithMessage("Password must contain at least one letter");
    }
}
