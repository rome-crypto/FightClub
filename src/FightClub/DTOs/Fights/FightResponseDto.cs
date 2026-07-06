using FightClub.DTOs.Boxers;
using FightClub.DTOs.Fights.FightDetails;

namespace FightClub.DTOs.Fights;

public class FightResponseDto
{
    public Guid Id { get; set; }

    public BoxerResponseDto BoxerA { get; set; } = new();
    public BoxerResponseDto BoxerB { get; set; } = new();

    public Guid? WinnerId { get; set; }

    public string Status { get; set; } = string.Empty;

    public int PlannedRounds { get; set; }

    public List<RoundsDto> Rounds { get; set; } = [];
}
