using FightClub.Entities.Fight;

namespace FightClub.DTOs.Fights.FightDetails;

public class FightDetailsDto
{
    public Guid Id { get; set; }

    public string BoxerAName { get; set; } = null!;
    public string BoxerBName { get; set; } = null!;

    public FightStatus Status { get; set; }

    public int PlannedRounds { get; set; }

    public List<RoundsDto> Rounds { get; set; } = new();
}
