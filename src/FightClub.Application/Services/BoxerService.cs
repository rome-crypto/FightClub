using AutoMapper;
using AutoMapper.QueryableExtensions;
using FightClub.Application.DTOs.Boxers;
using FightClub.Application.DTOs.Common;
using FightClub.Application.Exceptions;
using FightClub.Application.Interfaces;
using FightClub.Application.Specifications.Boxers;
using FightClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Application.Services;

public class BoxerService(IRepository<Boxer> repo, IMapper mapper)
        : IBoxerService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Boxer> _repository = repo;

    //command
    public async Task<BoxerResponseDto> CreateAsync(BoxerCreateDto dto)
    {
        var boxer = new Boxer(
            dto.FirstName, 
            dto.LastName, 
            dto.BirthDate, 
            dto.Weight, 
            dto.TrainerId);

        await _repository.AddAsync(boxer);
        await _repository.SaveChangesAsync();

        return _mapper.Map<BoxerResponseDto>(boxer);
    }

    //command
    public async Task DeleteAsync(Guid id)
    {
        Boxer boxer = await _repository.GetByIdAsync(id) 
            ?? throw new NotFoundException("Boxer not found");

        _repository.Delete(boxer);
        await _repository.SaveChangesAsync();
    }

    //query
    public async Task<BoxerResponseDto> GetByIdAsync(Guid id)
    {
        Boxer boxer = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Boxer not found");

        return _mapper.Map<BoxerResponseDto>(boxer);
    }

    //query
    public async Task<PagedResult<BoxerResponseDto>> GetPagedAsync(BoxerQueryDto query)
    {
        var spec = new BoxerSpecification(query);

        List<BoxerResponseDto> items = await _repository
            .Query(spec)
            .ProjectTo<BoxerResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var total = await _repository.CountAsync(spec);

        return new PagedResult<BoxerResponseDto>()
        {
            Items = items,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize            
        };
    }

    //command
    public async Task UpdateAsync(Guid id, BoxerUpdateDto dto)
    {
        Boxer boxer = await _repository.GetByIdAsync(id) 
            ?? throw new NotFoundException("Boxer not found");

        boxer.ChangeWeight(dto.Weight);
        boxer.ChangeBirthDate(dto.BirthDate);
        boxer.Rename(dto.FirstName, dto.LastName);
        boxer.AssignTrainer(dto.TrainerId);

        await _repository.SaveChangesAsync();
    }
}
