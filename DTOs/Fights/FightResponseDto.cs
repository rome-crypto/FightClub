namespace FightClub.DTOs.Fights;

public class FightResponseDto
{
    public Guid Id { get; set; }
    public string BoxerAName { get; set; } = string.Empty;
    public string BoxerBName { get; set; } = string.Empty;
    public string? WinnerName { get; set; }
    public DateTime FightDate { get; set; }
    public int Rounds { get; set; }
    public string? ResultMethod { get; set; }
}
