using System.Linq.Expressions;

namespace FightClub.Application.Specifications.Common;

public interface ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; }
    public Expression<Func<T, object>>? OrderBy { get; }
    public Expression<Func<T, object>>? OrderByDescending { get; }
    public IReadOnlyList<Expression<Func<T, object>>> Includes { get; }
    public int Take { get; }
    public int Skip { get; }
    public bool IsPagingEnabled { get; }
}
