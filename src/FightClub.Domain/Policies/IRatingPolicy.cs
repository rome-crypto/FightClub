using FightClub.Domain.Entities;
using FightClub.Domain.Enums;

namespace FightClub.Domain.Policies;

public interface IRatingPolicy
{
    (int WinnerRating, int LoserRating) Calculate(
        Boxer winner,
        Boxer loser);

    (int BoxerARating, int BoxerBRating) CalculateDraw(
        Boxer boxerA,
        Boxer boxerB);
}
