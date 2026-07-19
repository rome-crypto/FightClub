using AutoMapper;
using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Trainers;
using FightClub.Application.Interfaces;
using FightClub.Application.Exceptions;
using FightClub.Domain.Entities;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using FightClub.Application.Specifications.Trainers;

namespace FightClub.Application.Services;

public class TrainerService(
    IRepository<Trainer> repo,
    IMapper mapper) : ITrainerService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Trainer> _repository = repo;

    //command
    public async Task<TrainerResponseDto> CreateAsync(TrainerCreateDto dto)
    {
        var trainer = new Trainer(
            dto.FirstName,
            dto.LastName,
            dto.BirthDate);

        await _repository.AddAsync(trainer);
        await _repository.SaveChangesAsync();

        return _mapper.Map<TrainerResponseDto>(trainer);
    }

    //command
    public async Task DeleteAsync(Guid id)
    {
        Trainer trainer = await _repository
            .GetByIdAsync(id)
            ?? throw new NotFoundException("Trainer not found");

        _repository.Delete(trainer);
        await _repository.SaveChangesAsync();
    }

    //query
    public async Task<TrainerResponseDto> GetByIdAsync(Guid id)
    {
        Trainer trainer = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Trainer not found");

        return _mapper.Map<TrainerResponseDto>(trainer);
    }

    //query
    public async Task<PagedResult<TrainerResponseDto>> GetPagedAsync(TrainerQueryDto query)
    {
        var spec = new TrainerSpecification(query);

        List<TrainerResponseDto> items = await _repository
            .Query(spec)
            .ProjectTo<TrainerResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var total = await _repository
            .CountAsync(spec);

        return new PagedResult<TrainerResponseDto>()
        {
            Items = items,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    //command
    public async Task UpdateAsync(Guid id, TrainerUpdateDto dto)
    {
        Trainer trainer = await _repository
            .GetByIdAsync(id)
            ?? throw new NotFoundException("Boxer not found");

        trainer.ChangeBirthDate(dto.BirthDate);
        trainer.Rename(dto.FirstName, dto.LastName);

        await _repository.SaveChangesAsync();
    }
}
