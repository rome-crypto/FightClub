using FightClub.Application.DTOs.Auth;
using FluentValidation;

namespace FightClub.Application.Validators.Auth;

public sealed class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
{
    public RefreshTokenDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required");
    }
}
