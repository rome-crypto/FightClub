using FightClub.Domain.Entities;
using FightClub.Domain.Exceptions;
using FightClub.Domain.Policies;


namespace FightClub.Domain.Services;


public sealed class FightResultService
    : IFightResultService
{

    private readonly IRatingPolicy _ratingPolicy;


    public FightResultService(
        IRatingPolicy ratingPolicy)
    {
        _ratingPolicy = ratingPolicy;
    }



    public void Apply(
        Fight fight,
        Boxer boxerA,
        Boxer boxerB)
    {

        if (fight.EndType is null)
            throw new DomainException(
                "Fight is not finished.");



        var (BoxerARating, BoxerBRating) =
            _ratingPolicy.Calculate(
                boxerA,
                boxerB,
                fight.WinnerId);



        if (fight.WinnerId is null)
        {
            boxerA.ApplyFightResult(
                FightResult.Draw,
                fight.EndType.Value,
                BoxerARating);


            boxerB.ApplyFightResult(
                FightResult.Draw,
                fight.EndType.Value,
                BoxerBRating);


            return;
        }



        if (fight.WinnerId == boxerA.Id)
        {
            boxerA.ApplyFightResult(
                FightResult.Win,
                fight.EndType.Value,
                BoxerARating);


            boxerB.ApplyFightResult(
                FightResult.Loss,
                fight.EndType.Value,
                BoxerBRating);
        }
        else
        {
            boxerA.ApplyFightResult(
                FightResult.Loss,
                fight.EndType.Value,
                BoxerARating);


            boxerB.ApplyFightResult(
                FightResult.Win,
                fight.EndType.Value,
                BoxerBRating);
        }
    }
}