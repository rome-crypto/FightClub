using FightClub.Application.DTOs.Fights;
using FluentValidation;

namespace FightClub.Application.Validators.Fights;

public sealed class FightQueryValidator
    : AbstractValidator<FightQueryDto>
{
    public FightQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x)
            .Must(x => !x.From.HasValue ||
                       !x.To.HasValue ||
                       x.From <= x.To)
            .WithMessage("'From' must be earlier than 'To'.");
    }
}