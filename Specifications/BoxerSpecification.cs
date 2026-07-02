using System.Linq.Expressions;
using FightClub.Entities;
using FightClub.DTOs.Boxers;
using FightClub.DTOs.Common;

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
        if (!string.IsNullOrEmpty(query.WeightCategory))
        {
            AddCriteria(boxer => boxer.WeightCategory == query.WeightCategory
                && (!query.MinAge.HasValue || boxer.Age >= query.MinAge.Value)
                && (!query.MaxAge.HasValue || boxer.Age <= query.MaxAge.Value));
        }
        else
        {
            AddCriteria(boxer => (!query.MinAge.HasValue || boxer.Age >= query.MinAge.Value)
                && (!query.MaxAge.HasValue || boxer.Age <= query.MaxAge.Value));
        }

        ApplySorting(query.SortBy, query.SortOrder);

        int page = query.Page <= 0 ? 1 : query.Page;
        int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;
        ApplyPaging((page - 1) * pageSize, pageSize);

        AddInclude(x => x.Trainer!);
    }

    private void ApplySorting(string? sortBy, SortOrder sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy) || !SortSelectors.TryGetValue(sortBy, out var selector))
        {
            selector = x => x.LastName;
        }

        if (sortOrder == SortOrder.Desc)
            ApplyOrderByDescending(selector);
        else
            ApplyOrderBy(selector);
    }
}