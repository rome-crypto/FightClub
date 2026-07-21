using FightClub.Application.DTOs.Common;

namespace FightClub.Application.DTOs.Trainers;

public class TrainerQueryDto : BaseQueryDto
{
    public string? Search { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
}
