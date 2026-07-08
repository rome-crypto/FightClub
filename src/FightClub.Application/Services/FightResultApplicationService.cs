using FightClub.Application.Interfaces;
using FightClub.Domain.Entities;
using FightClub.Domain.Services;


namespace FightClub.Application.Services;


public sealed class FightResultApplicationService
    : IFightResultApplicationService
{

    private readonly FightResultService _domainService;



    public FightResultApplicationService(
        FightResultService domainService)
    {
        _domainService = domainService;
    }



    public void Apply(
        Fight fight,
        Boxer boxerA,
        Boxer boxerB)
    {
        _domainService.Apply(
            fight,
            boxerA,
            boxerB);
    }
}