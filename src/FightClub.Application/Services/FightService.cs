using AutoMapper;
using AutoMapper.QueryableExtensions;
using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Fights;
using FightClub.Application.Exceptions;
using FightClub.Application.Interfaces;
using FightClub.Application.Specifications.Fights;
using FightClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Application.Services;

public sealed class FightService(
    IRepository<Fight> fightRepository,
    IRepository<Boxer> boxerRepository,
    IMapper mapper) : IFightService
{
    private readonly IRepository<Fight> _fightRepository = fightRepository;
    private readonly IRepository<Boxer> _boxerRepository = boxerRepository;
    private readonly IMapper _mapper = mapper;

    //command
    public async Task<FightResponseDto> CreateAsync(
        FightCreateDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new EntityByIdsSpecification<Boxer>(
                    dto.BoxerAId,
                    dto.BoxerBId);

        List<Boxer> boxers = await _boxerRepository
            .Query(spec)
            .ToListAsync(cancellationToken);


        if (boxers.Count != 2)
        {
            throw new NotFoundException("Boxers not found");
        }

        var fight = new Fight(
            dto.BoxerAId,
            dto.BoxerBId, null,
            dto.Rounds);


        await _fightRepository.AddAsync(fight, cancellationToken);

        await _fightRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FightResponseDto>(fight);
    }

    //query
    public async Task<FightResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Fight fight = await _fightRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Fight not found");

        return _mapper.Map<FightResponseDto>(fight);
    }

    //comand
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Fight fight = await _fightRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Fight not found");

        fight.EnsureCanBeDeleted();

        _fightRepository.Delete(fight);

        await _fightRepository.SaveChangesAsync(cancellationToken);
    }

    //query
    public async Task<PagedResult<FightResponseDto>> GetPagedAsync(
        FightQueryDto query, CancellationToken cancellationToken = default)
    {
        var spec = new FightSpecification(query);

        var total = await _fightRepository
            .CountAsync(spec, cancellationToken);

        List<FightResponseDto> items = await _fightRepository
            .Query(spec)
            .ProjectTo<FightResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<FightResponseDto>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }
}
