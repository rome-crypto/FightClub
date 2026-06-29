using AutoMapper;
using FightClub.DTOs.Common;
using FightClub.DTOs.Trainers;
using FightClub.Entities;
using FightClub.Repositories.Interfaces;
using FightClub.Services.Interfaces;
using FightClub.Specifications;

namespace FightClub.Services;

public class TrainerService : ITrainerService
{
    private readonly IRepository<Trainer> _repo;
    private readonly IMapper _mapper;

    public TrainerService(IRepository<Trainer> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<TrainerResponseDto> CreateAsync(TrainerCreateDto dto)
    {
        var trainer = new Trainer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Age = dto.Age
        };

        await _repo.AddAsync(trainer);
        await _repo.SaveChangesAsync();

        return _mapper.Map<TrainerResponseDto>(trainer);
    }

    public async Task<TrainerResponseDto?> GetByIdAsync(Guid id)
    {
        var trainer = await _repo.GetByIdAsync(id);
        return trainer is null 
            ? null 
            : _mapper.Map<TrainerResponseDto>(trainer);
    }

    public async Task<PagedResult<TrainerResponseDto>> GetAsync(TrainerQueryDto query)
    {
        var spec = new TrainerSpecification(query);

        var trainers = await _repo.GetAllAsync(spec);
        var total = await _repo.CountAsync(spec);

        return new PagedResult<TrainerResponseDto>
        {
            Items = _mapper.Map<List<TrainerResponseDto>>(trainers),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    public async Task<TrainerResponseDto?> UpdateAsync(Guid id, TrainerUpdateDto dto)
    {
        var trainer = await _repo.GetByIdAsync(id);
        if (trainer is null) return null;

        trainer.FirstName = dto.FirstName;
        trainer.LastName = dto.LastName;
        trainer.Age = dto.Age;

        _repo.Update(trainer);
        await _repo.SaveChangesAsync();

        return _mapper.Map<TrainerResponseDto>(trainer);
    }

    public async Task DeleteAsync(Guid id)
    {
        var trainer = await _repo.GetByIdAsync(id);
        
        if (trainer is null) 
            return;

        _repo.Delete(trainer);
        await _repo.SaveChangesAsync();

    }
}
