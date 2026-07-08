using FightClub.Application.DTOs.Fights;
using FluentValidation;

namespace FightClub.Application.Validators.Fights;

public sealed class FightCreateDtoValidator
    : AbstractValidator<FightCreateDto>
{
    public FightCreateDtoValidator()
    {
        RuleFor(x => x.BoxerAId)
            .NotEmpty()
            .WithMessage("BoxerAId is required.");

        RuleFor(x => x.BoxerBId)
            .NotEmpty()
            .WithMessage("BoxerBId is required.");

        RuleFor(x => x)
            .Must(x => x.BoxerAId != x.BoxerBId)
            .WithMessage("A boxer cannot fight against themselves.");

        RuleFor(x => x.Rounds)
            .InclusiveBetween(1, 12)
            .WithMessage("Rounds must be between 1 and 12.");
    }
}