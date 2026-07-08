using AutoMapper;
using FightClub.Application.DTOs.Fights;
using FightClub.Application.Exceptions;
using FightClub.Application.Interfaces;
using FightClub.Application.Specifications;
using FightClub.Domain.Entities;
using FightClub.Domain.Policies;
using FightClub.Domain.Services;
using Microsoft.EntityFrameworkCore;


namespace FightClub.Application.Services;


public sealed class FightSimulationService
    : IFightSimulationService
{

    private readonly IRepository<Fight> _fightRepository;
    private readonly IRepository<Boxer> _boxerRepository;

    private readonly IRoundSimulator _roundSimulator;
    private readonly IFightEndingPolicy _endingPolicy;

    private readonly IFightResultService
        _resultService;

    private readonly IMapper _mapper;



    public FightSimulationService(
        IRepository<Fight> fightRepository,
        IRepository<Boxer> boxerRepository,
        IRoundSimulator roundSimulator,
        IFightEndingPolicy endingPolicy,
        IFightResultService resultService,
        IMapper mapper)
    {
        _fightRepository = fightRepository;
        _boxerRepository = boxerRepository;
        _roundSimulator = roundSimulator;
        _endingPolicy = endingPolicy;
        _resultService = resultService;
        _mapper = mapper;
    }



    public async Task<FightResponseDto> ExecuteAsync(
        Guid fightId)
    {

        var fight = await _fightRepository
            .Query(
                new FightByIdWithDetailsSpecification(
                    fightId))
            .FirstOrDefaultAsync()
            ??
            throw new NotFoundException(
                "Fight not found");



        var boxerA =
            await _boxerRepository
                .GetByIdAsync(fight.BoxerAId)
            ??
            throw new NotFoundException(
                "Boxer A not found");



        var boxerB =
            await _boxerRepository
                .GetByIdAsync(fight.BoxerBId)
            ??
            throw new NotFoundException(
                "Boxer B not found");



        fight.Start();



        while (fight.Status != FightStatus.Finished)
        {
            fight.StartRound();


            var score =
                _roundSimulator.Simulate(
                    fight.BoxerAId,
                    fight.BoxerBId);



            fight.EndCurrentRound(
                score,
                _endingPolicy);
        }



        _resultService.Apply(
            fight,
            boxerA,
            boxerB);



        await _fightRepository.SaveChangesAsync();



        return _mapper.Map<FightResponseDto>(fight);
    }
}