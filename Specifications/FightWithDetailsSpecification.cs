using FightClub.Entities.Fight;

namespace FightClub.Specifications;

public class FightWithDetailsSpecification : BaseSpecification<Fight>
{
    public FightWithDetailsSpecification(Guid fightId)
    {
        AddCriteria(x => x.Id == fightId);        
    }
}
