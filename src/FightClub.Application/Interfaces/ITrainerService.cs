using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Trainers;

namespace FightClub.Application.Interfaces;

public interface ITrainerService
{
    public Task<TrainerResponseDto> CreateAsync(TrainerCreateDto dto, CancellationToken cancellationToken);
    public Task<TrainerResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<PagedResult<TrainerResponseDto>> GetPagedAsync(TrainerQueryDto query, CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, TrainerUpdateDto dto, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
