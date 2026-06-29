using FightClub.DTOs.Boxers;

namespace FightClub.DTOs.Trainers;

public class TrainerWithBoxersDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<BoxerResponseDto> Boxers { get; set; } = new();
}
