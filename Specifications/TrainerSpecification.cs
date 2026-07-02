using FightClub.DTOs.Trainers;
using FightClub.Entities;
using FightClub.DTOs.Common;

namespace FightClub.Specifications;

public class TrainerSpecification : BaseSpecification<Trainer>
{
    /// <summary>
    /// Спецификация для фильтрации, сортировки и пагинации списка тренеров.
    /// Исключает Include тяжелых коллекций для предотвращения деградации производительности.
    /// </summary>
    public TrainerSpecification(TrainerQueryDto query)
    {
        if (!string.IsNullOrEmpty(query.Name))
        {
            var searchName = query.Name.Trim();
            AddCriteria(t =>
                t.FirstName.Contains(searchName) ||
                t.LastName.Contains(searchName));
        }

        if (query.MinAge.HasValue)
        {
            AddCriteria(t => t.Age >= query.MinAge.Value);
        }

        if (query.MaxAge.HasValue)
        {
            AddCriteria(t => t.Age <= query.MaxAge.Value);
        }

        if (!string.IsNullOrEmpty(query.SortBy) && query.SortBy.ToLower() == "age")
        {
            ApplySorting(t => t.Age, query.SortOrder == SortOrder.Desc);
        }
        else
        {
            ApplySorting(t => t.LastName, query.SortOrder == SortOrder.Desc);
        }

        ApplyPaging((query.Page - 1) * query.PageSize, query.PageSize);
    }
}