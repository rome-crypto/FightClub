using Microsoft.EntityFrameworkCore;

namespace FightClub.Application.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(
        IQueryable<T> query,
        ISpecification<T> spec)
        where T : class
    {
        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        query = spec.Includes.Aggregate(
            query,
            (current, include) => current.Include(include));

        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDescending != null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.IsPagingEnabled)
            query = query
                .Skip(spec.Skip)
                .Take(spec.Take);

        return query;
    }

    public static IQueryable<T> GetCountQuery<T>(
        IQueryable<T> query,
        ISpecification<T> spec)
        where T : class
    {
        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        return query;
    }
}