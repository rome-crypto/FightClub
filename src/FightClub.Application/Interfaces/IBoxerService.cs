using FightClub.Application.DTOs.Boxers;
using FightClub.Application.DTOs.Common;

namespace FightClub.Application.Interfaces;

public interface IBoxerService
{
    public Task<BoxerResponseDto> GetByIdAsync(Guid id);
    public Task<BoxerResponseDto> CreateAsync(BoxerCreateDto dto);
    public Task DeleteAsync(Guid id);
    public Task UpdateAsync(Guid id, BoxerUpdateDto dto);
    public Task<PagedResult<BoxerResponseDto>> GetPagedAsync(BoxerQueryDto query);
}
