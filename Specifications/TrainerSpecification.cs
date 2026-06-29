using FightClub.DTOs.Trainers;
using FightClub.Entities;
using FightClub.DTOs.Common;

namespace FightClub.Specifications;

public class TrainerSpecification : BaseSpecification<Trainer>
{
    public TrainerSpecification(TrainerQueryDto query)
    {
        AddInclude(x => x.Boxers);

        AddCriteria(t =>
            (string.IsNullOrEmpty(query.Name)
                || (t.FirstName + " " + t.LastName).Contains(query.Name))
            &&
            (!query.MinAge.HasValue || t.Age >= query.MinAge)
            &&
            (!query.MaxAge.HasValue || t.Age <= query.MaxAge)
        );

        ApplySorting(
            query.SortBy == "age"
                ? t => t.Age
                : t => t.LastName,
            query.SortOrder == SortOrder.Desc);

        ApplyPaging((query.Page - 1) * query.PageSize, query.PageSize);
    }
}
