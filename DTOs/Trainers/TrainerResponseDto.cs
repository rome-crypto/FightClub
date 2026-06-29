namespace FightClub.DTOs.Trainers;

public class TrainerResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public int BoxersCount { get; set; }
}
