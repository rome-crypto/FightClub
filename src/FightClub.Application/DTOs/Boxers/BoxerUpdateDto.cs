using FightClub.Domain.Enums;

namespace FightClub.Application.DTOs.Boxers;

public class BoxerUpdateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Age { get; set; }
    public WeightCategoryType? WeightCategory { get; set; }
}
