using FightClub.Entities;

namespace FightClub.Specifications;

public class EntityByIdsSpecification<T> : BaseSpecification<T> where T : IEntity
{
    public EntityByIdsSpecification(params Guid[] ids)
    {
        AddCriteria(x => ids.Contains(x.Id));
    }
}
