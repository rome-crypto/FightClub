using FightClub.Entities;
namespace FightClub.Specifications;

public class BoxerFilterSpecification : Specification<Boxer>
{
    public BoxerFilterSpecification(string? weightCategory, int? minAge, int? maxAge)
    {
        AddCriteria(x =>
            (weightCategory == null || x.WeightCategory == weightCategory) &&
            (!minAge.HasValue || x.Age >= minAge.Value) &&
            (!maxAge.HasValue || x.Age <= maxAge.Value)
        );
    }
}
