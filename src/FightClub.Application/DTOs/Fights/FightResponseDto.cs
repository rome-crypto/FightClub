using FightClub.Application.DTOs.Fights.FightDetails;
using FightClub.Domain.Enums;

namespace FightClub.Application.DTOs.Fights;

public class FightResponseDto
{
    public Guid Id { get; set; }

    public Guid BoxerAId { get; set; }

    public Guid BoxerBId { get; set; }

    public Guid? WinnerId { get; set; }

    public FightStatus Status { get; set; }

    public FightEndType? EndType { get; set; }

    public int PlannedRounds { get; set; }

    public int ActualRounds { get; set; }

    public int TotalScoreA { get; set; }

    public int TotalScoreB { get; set; }

    public IReadOnlyCollection<RoundsDto> Rounds { get; set; } = [];
}
