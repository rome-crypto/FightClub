using FightClub.Domain.Entities;

namespace FightClub.Application.Interfaces;

public interface IFightResultApplicationService
{
    public void Apply(
        Fight fight,
        Boxer boxerA,
        Boxer boxerB);
}