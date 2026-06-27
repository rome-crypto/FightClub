using FightClub.DTOs.Queries;
using FluentValidation;
namespace FightClub.Validators;

public class BoxerQueryValidator : AbstractValidator<BoxerQueryDto>
{
    public BoxerQueryValidator()
    {
        RuleFor(x => x.MinAge)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinAge.HasValue)
            .WithMessage("MinAge must be greater than or equal to 0.");
        
        RuleFor(x => x.MaxAge)
            .GreaterThanOrEqualTo(x => x.MinAge)
            .When(x => x.MaxAge.HasValue)
            .WithMessage("MaxAge must be greater than or equal to 0.");
        
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");
        
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 300)
            .WithMessage("PageSize must be inclusive between 1 anf 300");
        
        RuleFor(x => x.SortOrder)
            .Must(x => x == "asc" || x == "desc")
            .WithMessage("SortOrder must be either 'asc' or 'desc'.");
    }
}
