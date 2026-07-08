using AutoMapper;
using AutoMapper.QueryableExtensions;
using FightClub.Application.DTOs.Boxers;
using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Fights;
using FightClub.Application.Exceptions;
using FightClub.Application.Interfaces;
using FightClub.Application.Specifications;
using FightClub.Domain.Entities;
using FightClub.DTOs.Fights;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FightClub.Application.Services;

public sealed class FightService : IFightService
{
    private readonly IRepository<Fight> _fightRepository;
    private readonly IRepository<Boxer> _boxerRepository;
    private readonly IMapper _mapper;


    public FightService(
        IRepository<Fight> fightRepository,
        IRepository<Boxer> boxerRepository,
        IMapper mapper)
    {
        _fightRepository = fightRepository;
        _boxerRepository = boxerRepository;
        _mapper = mapper;
    }


    public async Task<FightResponseDto> CreateAsync(
        FightCreateDto dto)
    {
        var boxers = await _boxerRepository
            .Query(
                new EntityByIdsSpecification<Boxer>(
                    dto.BoxerAId,
                    dto.BoxerBId))
            .ToListAsync();


        if (boxers.Count != 2)
            throw new NotFoundException(
                "Boxers not found");


        var fight = new Fight(
            dto.BoxerAId,
            dto.BoxerBId,
            dto.Rounds);


        await _fightRepository.AddAsync(fight);

        await _fightRepository.SaveChangesAsync();


        return _mapper.Map<FightResponseDto>(fight);
    }



    public async Task<FightResponseDto> GetByIdAsync(Guid id)
    {
        var fight = await _fightRepository
            .GetByIdAsync(id)
            ??
            throw new NotFoundException(
                $"Fight {id} not found");


        return _mapper.Map<FightResponseDto>(fight);
    }



    public async Task DeleteAsync(Guid id)
    {
        var fight = await _fightRepository
            .GetByIdAsync(id)
            ??
            throw new NotFoundException(
                $"Fight {id} not found");


        _fightRepository.Delete(fight);

        await _fightRepository.SaveChangesAsync();
    }



    public async Task<PagedResult<FightResponseDto>> GetPagedAsync(
        FightQueryDto query)
    {
        var spec = new FightSpecification(query);


        var total =
            await _fightRepository.CountAsync(spec);


        var items =
            await _fightRepository
                .Query(spec)
                .ProjectTo<FightResponseDto>(
                    _mapper.ConfigurationProvider)
                .ToListAsync();



        return new PagedResult<FightResponseDto>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }
}