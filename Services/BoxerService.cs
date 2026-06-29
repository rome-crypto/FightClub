using AutoMapper;
using FightClub.DTOs.Boxers;
using FightClub.DTOs.Common;
using FightClub.Entities;
using FightClub.Exceptions;
using FightClub.Mappers;
using FightClub.Repositories.Interfaces;
using FightClub.Services.Interfaces;
using FightClub.Specifications;

namespace FightClub.Services;

public class BoxerService : IBoxerService
{
    private readonly IBoxerRepository _repo;
    private readonly IMapper _mapper;

    public BoxerService(IBoxerRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<BoxerResponseDto> CreateAsync(BoxerCreateDto dto)
    {
        var boxer = _mapper.Map<Boxer>(dto);
        
        await _repo.AddAsync(boxer);
        await _repo.SaveChangesAsync();
        
        return BoxerMapper.ToDto(boxer);
    }

    public async Task DeleteAsync(Guid id)
    {
        var boxer = await _repo.GetByIdAsync(id);

        if (boxer is null)
            throw new NotFoundException($"Boxer with ID ({id}) not found");

        _repo.Delete(boxer);
        await _repo.SaveChangesAsync();
    }

    public async Task<BoxerResponseDto> GetByIdAsync(Guid id)
    {
        var boxer = await _repo.GetByIdAsync(id);

        return boxer is null 
            ? throw new NotFoundException($"Boxer with ID ({id}) not found") 
            : BoxerMapper.ToDto(boxer);
    }

    public async Task<BoxerResponseDto> UpdateAsync(Guid id, BoxerUpdateDto dto)
    {
        var boxer = await _repo.GetByIdAsync(id);

        if (boxer is null) 
            throw new NotFoundException($"Boxer with ID ({id}) not found");

        _mapper.Map(dto, boxer);

        _repo.Update(boxer);
        await _repo.SaveChangesAsync();

        return BoxerMapper.ToDto(boxer);
    }

    public async Task<PagedResult<BoxerResponseDto>> GetAllAsync(
        BoxerQueryDto query)
    {
        var spec = new BoxerSpecification(query);

        var result = await _repo.GetPagedAsync(spec);

        return _mapper.Map<PagedResult<BoxerResponseDto>>(result);
    }
}
