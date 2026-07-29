using FightClub.Application.DTOs.Boxers;
using FightClub.Application.DTOs.Common;

namespace FightClub.Application.Interfaces;

public interface IBoxerService
{
    public Task<BoxerResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<BoxerResponseDto> CreateAsync(BoxerCreateDto dto, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    public Task UpdateAsync(Guid id, BoxerUpdateDto dto, CancellationToken cancellationToken);
    public Task<PagedResult<BoxerResponseDto>> GetPagedAsync(BoxerQueryDto query, CancellationToken cancellationToken);
}
