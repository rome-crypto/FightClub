using FightClub.Domain.Enums;
using FightClub.Domain.ValueObjects;

namespace FightClub.Application.DTOs.Boxers;

public class BoxerCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public WeightCategoryType? WeightCategory { get; set; }
}
