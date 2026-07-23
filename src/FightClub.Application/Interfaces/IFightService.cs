using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Fights;

namespace FightClub.Application.Interfaces;

public interface IFightService
{
    public Task<FightResponseDto> CreateAsync(FightCreateDto dto);
    public Task<FightResponseDto> GetByIdAsync(Guid id);
    public Task<PagedResult<FightResponseDto>> GetPagedAsync(FightQueryDto query);
    public Task DeleteAsync(Guid id);
}
