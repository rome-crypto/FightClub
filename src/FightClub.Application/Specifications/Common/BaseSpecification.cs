using System.Linq.Expressions;

namespace FightClub.Application.Specifications.Common;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; private set; }

    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;
    private readonly List<Expression<Func<T, object>>> _includes = [];

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(params Expression<Func<T, object>>[] includes)
    {
        _includes.AddRange(includes);
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
        Skip = skip < 0 ? 0 : skip;
        Take = take <= 0 ? 10 : take;
        IsPagingEnabled = true;
    }
}
