using FightClub.Entities;
using FightClub.DTOs.Fights;
namespace FightClub.Specifications;

public class FightSpecification : BaseSpecification<Fight>
{
    public FightSpecification(FightQueryDto query)
    {
        AddCriteria(f =>
            (!query.BoxerId.HasValue ||
                f.BoxerAId == query.BoxerId || f.BoxerBId == query.BoxerId)
            &&
            (!query.WinnerId.HasValue ||
                f.WinnerId == query.WinnerId)
            &&
            (!query.From.HasValue ||
                f.FightDate >= query.From)
            &&
            (!query.To.HasValue ||
                f.FightDate <= query.To)
        );

        ApplyPaging((query.Page - 1) * query.PageSize, query.PageSize);

        AddInclude(x => x.BoxerA);
        AddInclude(x => x.BoxerB);
        AddInclude(x => x.Winner);
    }
}
