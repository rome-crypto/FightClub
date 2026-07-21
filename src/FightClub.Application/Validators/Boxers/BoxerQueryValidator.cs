using FightClub.Application.DTOs.Boxers;
using FluentValidation;

namespace FightClub.Application.Validators.Boxers;

public sealed class BoxerQueryValidator
    : AbstractValidator<BoxerQueryDto>
{
    public BoxerQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.MinAge)
            .InclusiveBetween(18, 80)
            .When(x => x.MinAge.HasValue)
            .WithMessage("Minimum age must be between 18 and 80.");

        RuleFor(x => x.MaxAge)
            .InclusiveBetween(18, 80)
            .When(x => x.MaxAge.HasValue)
            .WithMessage("Maximum age must be between 18 and 80.");

        RuleFor(x => x)
            .Must(x =>
                !x.MinAge.HasValue ||
                !x.MaxAge.HasValue ||
                x.MinAge <= x.MaxAge)
            .WithMessage("Minimum age cannot be greater than maximum age.");
    }
}
