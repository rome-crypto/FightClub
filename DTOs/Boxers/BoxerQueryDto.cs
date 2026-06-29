using FightClub.DTOs.Common;

namespace FightClub.DTOs.Boxers;

public class BoxerQueryDto : BaseQueryDto
{
    public string? WeightCategory { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
}
