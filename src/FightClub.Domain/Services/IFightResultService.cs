using FightClub.Domain.Entities;

namespace FightClub.Domain.Services;

public interface IFightResultService
{
    public void Apply(
        Fight fight,
        Boxer boxerA,
        Boxer boxerB);
}