using AutoMapper;
using AutoMapper.QueryableExtensions;
using FightClub.DTOs.Boxers;
using FightClub.DTOs.Common;
using FightClub.DTOs.Fights;
using FightClub.Engine;
using FightClub.Entities;
using FightClub.Entities.Fight;
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
    private readonly IFightEngine _engine;
    private readonly IMapper _mapper;

    public FightService(
        IRepository<Fight> fightRepo,
        IRepository<Boxer> boxerRepo,
        IFightEngine engine,
        IMapper mapper)
    {
        _fightRepo = fightRepo;
        _boxerRepo = boxerRepo;
        _engine = engine;
        _mapper = mapper;
    }

    public async Task<FightResponseDto> CreateAndExecuteAsync(FightCreateDto dto)
    {
        if (dto.BoxerAId == dto.BoxerBId)
            throw new BusinessException("Boxer cannot fight himself");

        var boxers = await _boxerRepo
            .Query(new EntityByIdsSpecification<Boxer>(dto.BoxerAId, dto.BoxerBId))
            .ToListAsync();

        if (boxers.Count != 2)
            throw new NotFoundException("Boxers not found");

        var boxerA = boxers.First(x => x.Id == dto.BoxerAId);
        var boxerB = boxers.First(x => x.Id == dto.BoxerBId);

        var fight = new Fight
        {
            BoxerAId = boxerA.Id,
            BoxerBId = boxerB.Id,
            PlannedRounds = dto.Rounds,
            Status = FightStatus.Scheduled,
            CreatedAt = DateTime.UtcNow
        };

        fight = _engine.Execute(fight);

        await _fightRepo.AddAsync(fight);
        await _fightRepo.SaveChangesAsync();

        return _mapper.Map<FightResponseDto>(fight);
    }

    public async Task<PagedResult<FightResponseDto>> GetPagedAsync(FightQueryDto query)
    {
        var spec = new FightSpecification(query);

        var total = await _fightRepo.CountAsync(spec);

        var items = await _fightRepo
            .Query(spec)
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

    public async Task<FightResponseDto> GetByBoxerIdAsync(Guid id)
    {
        var spec = new FightWithDetailsSpecification(id);

        var fight = await _fightRepo
            .Query(spec)
            .ProjectTo<FightResponseDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException($"Fight with ID ({id}) not found");

        return fight;
    }

    public async Task<FightResponseDto> GetByIdAsync(Guid id)
    {
        var boxer = await _fightRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Boxer with ID ({id}) not found"); ;

        return _mapper.Map<FightResponseDto>(boxer);
    }

    public async Task DeleteAsync(Guid id)
    {
        var fight = await _fightRepo.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Fight with ID ({id}) not found");

        _fightRepo.Delete(fight);
        await _fightRepo.SaveChangesAsync();;
    }

    public async Task<FightResponseDto> UpdateAsync(Guid id, FightUpdateDto dto)
    {
        var fight = await _fightRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Fight with ID ({id}) not found");

        _mapper.Map(dto, fight);

        await _fightRepo.SaveChangesAsync();

        return _mapper.Map<FightResponseDto>(fight);
    }
}