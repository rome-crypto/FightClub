using FightClub.DTOs.Boxers;
using FightClub.DTOs.Common;
using FightClub.Entities;
using System.Linq.Expressions;

namespace FightClub.Specifications;

public class BoxerSpecification : BaseSpecification<Boxer>
{
    private static readonly Dictionary<string, Expression<Func<Boxer, object>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["firstname"] = x => x.FirstName,
            ["lastname"] = x => x.LastName,
            ["age"] = x => x.Age,
            ["weightcategory"] = x => x.WeightCategory
        };

    public BoxerSpecification(BoxerQueryDto query)
    {
        AddCriteria(x =>
            (string.IsNullOrWhiteSpace(query.Search) ||
             x.FirstName.Contains(query.Search) ||
             x.LastName.Contains(query.Search)) &&

            (string.IsNullOrWhiteSpace(query.WeightCategory) ||
             x.WeightCategory == query.WeightCategory) &&

            (!query.MinAge.HasValue || x.Age >= query.MinAge.Value) &&
            (!query.MaxAge.HasValue || x.Age <= query.MaxAge.Value)
        );

        var sortBy = query.SortBy;

        if (!SortMap.TryGetValue(sortBy ?? "lastname", out var selector))
            selector = x => x.LastName;

        if (query.SortOrder == SortOrder.Desc)
            ApplyOrderByDescending(selector);
        else
            ApplyOrderBy(selector);

        ApplyPaging(query.Page, query.PageSize);
    }
}