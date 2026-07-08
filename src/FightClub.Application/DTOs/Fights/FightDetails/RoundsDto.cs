namespace FightClub.Application.DTOs.Fights.FightDetails;

public class RoundsDto
{
    public int Number { get; set; }

    public int ScoreA { get; set; }
    public int ScoreB { get; set; }

    public List<RoundEventDto> Events { get; set; } = new();
}