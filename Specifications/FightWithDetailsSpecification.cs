using FightClub.Entities.Fight;

namespace FightClub.Specifications;

public class FightWithDetailsSpecification : BaseSpecification<Fight>
{
    public FightWithDetailsSpecification(Guid id)
    {
        AddCriteria(f => f.Id == id);

        AddInclude(f => f.BoxerA);
        AddInclude(f => f.BoxerB);
        AddInclude(f => f.Rounds);
    }
}
