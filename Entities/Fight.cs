namespace FightClub.Entities;

public class Fight
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BoxerAId { get; set; }
    public Boxer BoxerA { get; set; } = null!;

    public Guid BoxerBId { get; set; }
    public Boxer BoxerB { get; set; } = null!;

    public Guid? WinnerId { get; set; }
    public Boxer? Winner { get; set; } = null!;

    public DateTime FightDate { get; set; } = DateTime.UtcNow;

    public int Rounds { get; set; }

    public string? ResultMethod { get; set; }
}
