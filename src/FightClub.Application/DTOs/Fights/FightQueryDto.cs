using FightClub.Domain.Services;
using FightClub.Application.DTOs.Common;

namespace FightClub.Application.DTOs.Fights;

public class FightQueryDto : BaseQueryDto
{
    public FightStatus? Status { get; set; }
    public Guid? BoxerId { get; set; }
    public Guid? WinnerId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}
