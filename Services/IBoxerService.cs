using FightClub.DTOs;

namespace FightClub.Services;

public interface IBoxerService
{
    public Task<List<BoxerResponseDto>> GetAllAsync();
    public Task<BoxerResponseDto> GetByIdAsync(Guid id);
    public Task<BoxerResponseDto> CreateAsync(BoxerCreateDto dto);
    public Task DeleteAsync(Guid id);
    public Task<BoxerResponseDto> UpdateAsync(Guid id, BoxerUpdateDto dto);
    public Task<List<BoxerResponseDto>> GetFilteredAsync(string? weightCategory, int? minAge, int? maxAge);
}
