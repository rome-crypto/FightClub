using FightClub.Application.DTOs.Boxers;

namespace FightClub.Application.DTOs.Trainers;

public class TrainerWithBoxersDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<BoxerResponseDto> Boxers { get; set; } = new();
}
