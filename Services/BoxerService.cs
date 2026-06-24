using FightClub.DTOs;
using FightClub.Entities;
using FightClub.Exceptions;
using FightClub.Mappers;
using FightClub.Repositories.Interfaces;
using FightClub.Specifications;

namespace FightClub.Services;

public class BoxerService : IBoxerService
{
    private readonly IBoxerRepository _repo;

    public BoxerService(IBoxerRepository repo)
    {
        _repo = repo;
    }

    public async Task<BoxerResponseDto> CreateAsync(BoxerCreateDto dto)
    {
        var boxer = new Boxer(dto.FirstName, dto.LastName, dto.Age, dto.WeightCategory);
        
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

    public async Task<List<BoxerResponseDto>> GetAllAsync()
    {
        var boxers = await _repo.GetAllAsync();

        return boxers
            .Select(BoxerMapper.ToDto)
            .ToList();
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


        if (dto.FirstName is not null)
            boxer.FirstName = dto.FirstName;

        if (dto.LastName is not null)
            boxer.LastName = dto.LastName;

        if (dto.Age is not null)
            boxer.Age = dto.Age.Value;

        if (dto.WeightCategory is not null)
            boxer.WeightCategory = dto.WeightCategory;

        _repo.Update(boxer);
        await _repo.SaveChangesAsync();

        return BoxerMapper.ToDto(boxer);
    }

    public async Task<List<BoxerResponseDto>> GetFilteredAsync(
        string? weightCategory,
        int? minAge, 
        int? maxAge)
    {
        var spec = new BoxerFilterSpecification(weightCategory, minAge, maxAge);

        var boxers = await _repo.GetAllAsync(spec.Criteria);

        return boxers.Select(BoxerMapper.ToDto).ToList();
    }
}
