using FightClub.Domain.Entities;
using FightClub.Domain.Enums;

namespace FightClub.Domain.Policies;

public interface IFightEndingPolicy
{
    bool TryFinish(
        Fight fight,
        out Guid? winnerId,
        out FightEndType endType);
}
