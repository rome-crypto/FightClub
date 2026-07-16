using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Trainers;

namespace FightClub.Application.Interfaces;

public interface ITrainerService
{
    Task<TrainerResponseDto> CreateAsync(TrainerCreateDto dto);
    Task<TrainerResponseDto> GetByIdAsync(Guid id);
    Task<PagedResult<TrainerResponseDto>> GetPagedAsync(TrainerQueryDto query);
    Task UpdateAsync(Guid id, TrainerUpdateDto dto);
    Task DeleteAsync(Guid id);
}
