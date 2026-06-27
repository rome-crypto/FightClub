using FightClub.DTOs;
using FightClub.DTOs.Common;
using FightClub.DTOs.Queries;

namespace FightClub.Services;

public interface IBoxerService
{
    public Task<BoxerResponseDto> GetByIdAsync(Guid id);
    public Task<BoxerResponseDto> CreateAsync(BoxerCreateDto dto);
    public Task DeleteAsync(Guid id);
    public Task<BoxerResponseDto> UpdateAsync(Guid id, BoxerUpdateDto dto);
    public Task<PagedResult<BoxerResponseDto>> GetAllAsync(BoxerQueryDto query);
}
