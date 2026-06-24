using System.Linq.Expressions;

namespace FightClub.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
}
