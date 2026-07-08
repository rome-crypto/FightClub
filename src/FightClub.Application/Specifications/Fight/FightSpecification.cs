using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Fights;
using FightClub.Domain.Entities;
using System.Linq.Expressions;

namespace FightClub.Application.Specifications;

public class FightSpecification : BaseSpecification<Fight>
{
    private readonly Dictionary<string, Expression<Func<Fight, object>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["date"] = f => f.CreatedAt,
            ["status"] = f => f.Status,
        };

    public FightSpecification(FightQueryDto query)
    {
        AddCriteria(x =>
            (!query.Status.HasValue || x.Status == query.Status) &&
            (!query.BoxerId.HasValue || x.BoxerAId == query.BoxerId || x.BoxerBId == query.BoxerId) &&
            (!query.WinnerId.HasValue || x.WinnerId == query.WinnerId) &&
            (!query.From.HasValue || x.CreatedAt >= query.From) &&
            (!query.To.HasValue || x.CreatedAt <= query.To)
        );

        var sortBy = query.SortBy ?? "date";

        if (!SortMap.TryGetValue(sortBy, out var selector))
            selector = x => x.CreatedAt;

        if (query.SortOrder == SortOrder.Desc)
            ApplyOrderByDescending(selector);
        else
            ApplyOrderBy(selector);

        ApplyPaging((query.Page - 1) * query.PageSize, query.PageSize);
    }
}