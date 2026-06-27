using FightClub.DTOs.Queries;
using System.Linq.Expressions;
using FightClub.Entities;
namespace FightClub.Specifications;

public class BoxerSpecification : BaseSpecification<Boxer>
{
    private static readonly Dictionary<string, Expression<Func<Boxer, object>>> SortSelectors =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["firstname"] = x => x.FirstName,
            ["lastname"] = x => x.LastName,
            ["age"] = x => x.Age,
            ["weightcategory"] = x => x.WeightCategory
        };
    
    public BoxerSpecification(BoxerQueryDto query)
    {
        AddCriteria(boxer =>
            (string.IsNullOrEmpty(query.WeightCategory)
                || boxer.WeightCategory == query.WeightCategory)
            &&
            (!query.MinAge.HasValue
                || boxer.Age >= query.MinAge.Value)
            &&
            (!query.MaxAge.HasValue
                || boxer.Age <= query.MaxAge.Value)
        );

        ApplySorting(query);

        ApplyPaging((query.Page -1) * query.PageSize, query.PageSize);
    }
    private void ApplySorting(BoxerQueryDto query)
    {
        if (!SortSelectors.TryGetValue(query.SortBy ?? "lastname", out var selector))
        {
            selector = x => x.LastName;
        }

        if (query.SortOrder.Equals("decs", StringComparison.OrdinalIgnoreCase))
            ApplyOrderByDescending(selector);
        else
            ApplyOrderBy(selector);
    }

}
