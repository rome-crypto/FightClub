using FightClub.Application.Specifications.Common;
using FightClub.Domain.Common;

namespace FightClub.Application.Specifications.Fights;

public class EntityByIdsSpecification<T> : BaseSpecification<T> where T : Entity
{
    public EntityByIdsSpecification(params Guid[] ids)
    {
        AddCriteria(x => ids.Contains(x.Id));
    }
}
