using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Fights;

namespace FightClub.Application.Interfaces;

public interface IFightService
{
    Task<FightResponseDto> CreateAsync(FightCreateDto dto);
    Task<FightResponseDto> GetByIdAsync(Guid id);
    Task<PagedResult<FightResponseDto>> GetPagedAsync(FightQueryDto query);
    Task DeleteAsync(Guid id);
}
