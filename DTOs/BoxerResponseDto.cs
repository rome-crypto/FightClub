namespace FightClub.DTOs;

public class BoxerResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string WeightCategory { get; set; } = string.Empty;
}
