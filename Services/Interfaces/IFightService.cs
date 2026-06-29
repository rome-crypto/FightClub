using FightClub.DTOs.Common;
using FightClub.DTOs.Fights;
using FightClub.DTOs.Trainers;

namespace FightClub.Services.Interfaces;

public interface IFightService
{
    Task<FightResponseDto> CreateAsync(FightCreateDto dto);
    Task<FightResponseDto?> GetByIdAsync(Guid id);
    Task<PagedResult<FightResponseDto>> GetAsync(FightQueryDto query);
    Task<FightResponseDto?> UpdateAsync(Guid id, FightUpdateDto dto);
    Task DeleteAsync(Guid id);
}
