using FightClub.Application.DTOs.Common;

namespace FightClub.Application.DTOs.Boxers;

public class BoxerQueryDto : BaseQueryDto
{
    public string? Search { get; set; }
    public string? WeightCategory { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
}
