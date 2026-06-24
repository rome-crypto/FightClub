using System.Linq.Expressions;

namespace FightClub.Specifications;

public class Specification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
}
