using FightClub.Models;

namespace FightClub.Services;

public interface IBoxerService
{
    public Task<IEnumerable<Boxer>> GetAllBoxers();
    public Task<Boxer?> GetBoxerById(Guid id);
    public Task<Boxer> CreateBoxer(BoxerCreateDto dto);
    public Task DeleteBoxer(Guid id);
    public Task<Boxer?> UpdateBoxer(Guid id, BoxerUpdateDto dto);
}
