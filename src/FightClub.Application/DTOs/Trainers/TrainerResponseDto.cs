namespace FightClub.Application.DTOs.Trainers;

public class TrainerResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public int BoxersCount { get; set; }
}
