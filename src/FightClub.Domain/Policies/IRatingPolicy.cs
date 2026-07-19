using FightClub.Domain.Entities;

namespace FightClub.Domain.Policies;

public interface IRatingPolicy
{
    public (int BoxerARating,
        int BoxerBRating)
    Calculate(
        Boxer boxerA,
        Boxer boxerB,
        Guid? winnerId);
}
