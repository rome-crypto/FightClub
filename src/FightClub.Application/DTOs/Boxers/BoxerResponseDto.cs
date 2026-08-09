namespace FightClub.Application.DTOs.Boxers;

public class BoxerResponseDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    public int Age { get; set; }

    public int Weight { get; set; }
    public string WeightCategory { get; set; } = string.Empty;

    public Guid? TrainerId { get; set; }

    public int EloRating { get; set; }

    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }

    public double WinRate { get; set; }
}
