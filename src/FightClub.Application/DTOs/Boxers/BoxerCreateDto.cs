using FightClub.Domain.Services;
using FightClub.Domain.ValueObjects;

namespace FightClub.Application.DTOs.Boxers;

public class BoxerCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public int Weight { get; set; }

    public Guid? TrainerId { get; set; }
}
