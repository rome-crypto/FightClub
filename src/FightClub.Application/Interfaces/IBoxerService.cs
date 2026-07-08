using FightClub.DTOs.Boxers;
using FightClub.DTOs.Common;

namespace FightClub.Services.Interfaces;

public interface IBoxerService
{
    Task<BoxerResponseDto> GetByIdAsync(Guid id);
    Task<BoxerResponseDto> CreateAsync(BoxerCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<BoxerResponseDto> UpdateAsync(Guid id, BoxerUpdateDto dto);
    Task<PagedResult<BoxerResponseDto>> GetPagedAsync(BaseQueryDto query);
}
