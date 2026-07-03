using FightClub.DTOs.Common;
using FightClub.DTOs.Trainers;
using FightClub.Entities;
using System.Linq.Expressions;

namespace FightClub.Specifications;

public class TrainerSpecification : BaseSpecification<Trainer>
{
    private static readonly Dictionary<string, Expression<Func<Trainer, object>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["firstname"] = x => x.FirstName,
            ["lastname"] = x => x.LastName,
            ["age"] = x => x.Age
        };

    public TrainerSpecification(TrainerQueryDto query)
    {
        AddCriteria(x =>
            (string.IsNullOrWhiteSpace(query.Search) ||
            x.FirstName.Contains(query.Search) ||
            x.LastName.Contains(query.Search)) &&

            (!query.MinAge.HasValue || x.Age >= query.MinAge.Value) &&
            (!query.MaxAge.HasValue || x.Age <= query.MaxAge.Value)
        );

        var sortBy = query.SortBy;

        if (!SortMap.TryGetValue(sortBy ?? "name", out var selector))
            selector = x => x.LastName;

        if (query.SortOrder == SortOrder.Desc)
            ApplyOrderByDescending(selector);
        else
            ApplyOrderBy(selector);

        ApplyPaging((query.Page - 1) * query.PageSize, query.PageSize);
    }
}