namespace FightClub.DTOs.Fights;

public class FightRoundDto
{
    public int Number { get; set; }

    public int ScoreA { get; set; }
    public int ScoreB { get; set; }

    public string Result { get; set; } = string.Empty;

    public List<RoundEventDto> Events { get; set; } = new();
}
