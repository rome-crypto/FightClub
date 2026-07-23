namespace FightClub.Application.DTOs.Boxers;

public class BoxerUpdateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public int? Weight { get; set; }

    public Guid? TrainerId { get; set; }
}
