using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Trainers;
using FluentValidation;

namespace FightClub.Application.Validators.Trainer;

public sealed class TrainerQueryValidator
    : AbstractValidator<TrainerQueryDto>
{
    public TrainerQueryValidator()
    {
        RuleFor(x => x.MinAge)
            .GreaterThanOrEqualTo(18)
            .When(x => x.MinAge.HasValue)
            .WithMessage("MinAge must be at least 18.");

        RuleFor(x => x.MaxAge)
            .GreaterThanOrEqualTo(x => x.MinAge ?? 18)
            .When(x => x.MaxAge.HasValue)
            .WithMessage("MaxAge must be greater than or equal to MinAge.");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}