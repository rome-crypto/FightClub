using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Trainers;

namespace FightClub.Application.Interfaces;

public interface ITrainerService
{
    public Task<TrainerResponseDto> CreateAsync(TrainerCreateDto dto);
    public Task<TrainerResponseDto> GetByIdAsync(Guid id);
    public Task<PagedResult<TrainerResponseDto>> GetPagedAsync(TrainerQueryDto query);
    public Task UpdateAsync(Guid id, TrainerUpdateDto dto);
    public Task DeleteAsync(Guid id);
}
