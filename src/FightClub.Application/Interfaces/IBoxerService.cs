using FightClub.Application.DTOs.Boxers;
using FightClub.Application.DTOs.Common;

namespace FightClub.Application.Interfaces;

public interface IBoxerService
{
    Task<BoxerResponseDto> GetByIdAsync(Guid id);
    Task<BoxerResponseDto> CreateAsync(BoxerCreateDto dto);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, BoxerUpdateDto dto);
    Task<PagedResult<BoxerResponseDto>> GetPagedAsync(BoxerQueryDto query);
}
