using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Fights;

namespace FightClub.Application.Interfaces;

public interface IFightService
{
    public Task<FightResponseDto> CreateAsync(FightCreateDto dto, CancellationToken cancellationToken);
    public Task<FightResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<PagedResult<FightResponseDto>> GetPagedAsync(FightQueryDto query, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
