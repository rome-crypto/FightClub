using FightClub.Domain.Entities;

namespace FightClub.Application.Specifications;

public class FightByIdWithDetailsSpecification
    : BaseSpecification<Fight>
{
    public FightByIdWithDetailsSpecification(Guid id)
    {
        AddCriteria(x => x.Id == id);
        AddInclude(x => x.Rounds);
    }
}