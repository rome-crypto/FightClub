using FightClub.Entities.Fight;

namespace FightClub.Specifications;

public class FightWithDetailsSpecification : BaseSpecification<Fight>
{
    public FightWithDetailsSpecification(Guid id)
    {
        AddCriteria(f => f.Id == id);

        AddInclude(f => f.BoxerA, 
            f => f.BoxerB, 
            f => f.Rounds);
    }
}
