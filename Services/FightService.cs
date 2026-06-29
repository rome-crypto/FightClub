using AutoMapper;
using AutoMapper.QueryableExtensions;
using FightClub.DTOs.Common;
using FightClub.DTOs.Fights;
using FightClub.DTOs.Trainers;
using FightClub.Entities;
using FightClub.Exceptions;
using FightClub.Repositories.Interfaces;
using FightClub.Services.Interfaces;
using FightClub.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Services;

public class FightService : IFightService
{
    private readonly IRepository<Fight> _fightRepo;
    private readonly IRepository<Boxer> _boxerRepo;
    private readonly IMapper _mapper;
    private readonly Random _random = new();

    public FightService(
        IRepository<Fight> fightRepo,
        IRepository<Boxer> boxerRepo,
        IMapper mapper)
    {
        _fightRepo = fightRepo;
        _boxerRepo = boxerRepo;
        _mapper = mapper;
    }

    public async Task<FightResponseDto> CreateAsync(FightCreateDto dto)
    {
        if (dto.BoxerAId == dto.BoxerBId)
            throw new BusinessException("Boxer cannot fight himself");

        var boxerA = await _boxerRepo.GetByIdAsync(dto.BoxerAId);
        var boxerB = await _boxerRepo.GetByIdAsync(dto.BoxerBId);

        if (boxerA is null || boxerB is null)
            throw new BusinessException("Boxers not found");

        var winner = SimulateFight(boxerA, boxerB);

        var fight = new Fight
        {
            BoxerAId = boxerA.Id,
            BoxerBId = boxerB.Id,
            WinnerId = winner?.Id,
            Rounds = dto.Rounds,
            FightDate = DateTime.UtcNow,
            ResultMethod = winner == null 
                ? "Draw" 
                : "Decision"
        };

        await _fightRepo.AddAsync(fight);
        await _fightRepo.SaveChangesAsync();

        return _mapper.Map<FightResponseDto>(fight);
    }

    public async Task<PagedResult<FightResponseDto>> GetAsync(FightQueryDto query)
    {
        var spec = new FightSpecification(query);

        var baseQuery = _fightRepo.Query(spec);

        var total = await baseQuery.CountAsync();

        var items = await baseQuery
            .ProjectTo<FightResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<FightResponseDto>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    private Boxer? SimulateFight(Boxer a, Boxer b)
    {
        var powerA = CalculatePower(a) * RandomFactor();
        var powerB = CalculatePower(b) * RandomFactor();

        if (Math.Abs(powerA - powerB) < 5)
            return null;

        return powerA > powerB ? a : b;
    }

    private double CalculatePower(Boxer boxer)
    { 
        return 100 - boxer.Age; 
    }

    private double RandomFactor()
    { 
        return 0.8 + _random.NextDouble() * 0.4; 
    }

    public async Task<FightResponseDto?> GetByIdAsync(Guid id)
    {
        var fight = await _fightRepo.GetByIdAsync(id);
        return fight is null 
            ? null 
            : _mapper.Map<FightResponseDto>(fight);
    }

    public async Task DeleteAsync(Guid id)
    {
        var fight = await _fightRepo.GetByIdAsync(id);
        
        if (fight is null) 
            return;

        _fightRepo.Delete(fight);
        await _fightRepo.SaveChangesAsync();;
    }

    public async Task<FightResponseDto?> UpdateAsync(Guid id, FightUpdateDto dto)
    {
        var fight = await _fightRepo.GetByIdAsync(id);
        if (fight is null) 
            return null;

        fight.Rounds = dto.Rounds;
        fight.FightDate = dto.FightDate;
        fight.ResultMethod = dto.ResultMethod;

        _fightRepo.Update(fight);
        await _fightRepo.SaveChangesAsync();

        return _mapper.Map<FightResponseDto>(fight);
    }
}