using FightClub.Domain.Enums;

namespace FightClub.Application.DTOs.Fights.FightDetails;

public class RoundEventDto
{
    public RoundEventType Type { get; set; }
    public Guid? BoxerId { get; set; }
    public DateTime OccurredAt { get; set; }
}
