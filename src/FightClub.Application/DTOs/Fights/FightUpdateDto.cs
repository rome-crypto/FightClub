namespace FightClub.Application.DTOs.Fights;

public class FightUpdateDto
{
    public int Rounds { get; set; }
    public DateTime FightDate { get; set; }
    public string ResultMethod { get; set; } = string.Empty;
}
