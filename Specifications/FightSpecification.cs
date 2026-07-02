using FightClub.DTOs.Fights;
using FightClub.Entities.Fight;

namespace FightClub.Specifications;

public class FightSpecification : BaseSpecification<Fight>
{
    public FightSpecification(FightQueryDto query)
    {
        AddCriteria(f =>
            !query.BoxerId.HasValue || f.BoxerAId == query.BoxerId || f.BoxerBId == query.BoxerId
        );

        ApplyOrderByDescending(f => f.Id);

        int page = query.Page <= 0 ? 1 : query.Page;
        int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;
        int skip = (page - 1) * pageSize;

        ApplyPaging(skip, pageSize);

        AddInclude(x => x.BoxerA, x => x.BoxerB);
    }
}