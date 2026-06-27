using FightClub.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Repositories;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(
        IQueryable<T> inputQuery, 
        ISpecification<T> spec) 
        where T : class
    {
        var query = inputQuery;

        query = ApplyCriteria(query, spec);
        query = ApplyIncludes(query, spec);
        query = ApplyOrdering(query, spec);
        query = ApplyPaging(query, spec);

        return query;
    }

    public static IQueryable<T> GetCountQuery<T>(
        IQueryable<T> inputQuery, 
        ISpecification<T> spec)
        where T : class
    {
        var query = inputQuery;
        query = ApplyCriteria(query, spec);
        query = ApplyIncludes(query, spec);

        return query;
    }

    private static IQueryable<T> ApplyCriteria<T>(
        IQueryable<T> query, 
        ISpecification<T> spec) 
        where T: class
    {
        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        return query;
    }

    private static IQueryable<T> ApplyIncludes<T>(
        IQueryable<T> query,
        ISpecification<T> spec)
        where T : class
    {
        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        return spec.Includes.Aggregate(
            query, 
            (current, include) => current.Include(include));
    }

    private static IQueryable<T> ApplyOrdering<T>(
        IQueryable<T> query,
        ISpecification<T> spec)
        where T : class
    {
        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);
        
        return query;
    }

    private static IQueryable<T> ApplyPaging<T>(
        IQueryable<T> query,
        ISpecification<T> spec)
        where T : class
    {
        if (spec.IsPagingEnabled)
            query = query
                .Skip(spec.Skip)
                .Take(spec.Take);

        return query;
    }
}
