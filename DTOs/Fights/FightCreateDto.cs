namespace FightClub.DTOs.Fights;

public class FightCreateDto
{
    public Guid BoxerAId { get; set; }
    public Guid BoxerBId { get; set; }
    public int Rounds { get; set; }
}
