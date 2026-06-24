namespace FightClub.DTOs;

public class BoxerCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string WeightCategory { get; set; } = string.Empty;
}
