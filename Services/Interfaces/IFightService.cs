using FightClub.DTOs.Common;
using FightClub.DTOs.Fights;
using FightClub.DTOs.Trainers;

namespace FightClub.Services.Interfaces;

public interface IFightService
{
    Task<FightResponseDto> CreateAndExecuteAsync(FightCreateDto dto);
    Task<FightResponseDto> GetByBoxerIdAsync(Guid id);
    Task<PagedResult<FightResponseDto>> GetPagedAsync(FightQueryDto query);
    Task<FightResponseDto> UpdateAsync(Guid id, FightUpdateDto dto);
    Task DeleteAsync(Guid id);
}
