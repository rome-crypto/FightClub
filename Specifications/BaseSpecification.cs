using System.Linq.Expressions;
using FightClub.DTOs.Common;

namespace FightClub.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public Expression<Func<T, object>>? OrderBy { get; protected set; }
    public Expression<Func<T, object>>? OrderByDescending { get; protected set; }
    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;
    public int Take { get; protected set; }
    public int Skip { get; protected set; }
    public bool IsPagingEnabled { get; protected set; }
    private readonly List<Expression<Func<T, object>>> _includes = new();

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderBy = orderBy;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc)
    {
        OrderByDescending = orderByDesc;
    }

    protected void ApplyPaging(int skip, int take)
    {
        if (skip < 0) skip = 0;
        if (take <= 0) take = 10;

        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplySorting( 
        Expression<Func<T, object>> selector,
        bool descending)
    {
        if (descending) 
            ApplyOrderByDescending(selector);
        else 
            ApplyOrderBy(selector);
    }

    protected void AddInclude(params Expression<Func<T, object>>[] includes)
    {
        _includes.AddRange(includes);
    }
}
