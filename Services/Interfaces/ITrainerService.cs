using FightClub.DTOs.Common;
using FightClub.DTOs.Trainers;

namespace FightClub.Services.Interfaces;

public interface ITrainerService
{
    Task<TrainerResponseDto> CreateAsync(TrainerCreateDto dto);
    Task<TrainerResponseDto> GetByIdAsync(Guid id);
    Task<PagedResult<TrainerResponseDto>> GetAsync(TrainerQueryDto query);
    Task<TrainerResponseDto> UpdateAsync(Guid id, TrainerUpdateDto dto);
    Task DeleteAsync(Guid id);
}
