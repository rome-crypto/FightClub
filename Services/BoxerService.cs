using FightClub.Data;
using FightClub.Models;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Services;

public class BoxerService : IBoxerService
{
    private readonly FightClubDbContext _db;

    public BoxerService(FightClubDbContext db)
    {
        _db = db;
    }

    public async Task<Boxer> CreateBoxer(BoxerCreateDto dto)
    {
        var boxer = new Boxer(dto.FirstName, dto.LastName, dto.Age, dto.WeightCategory);
        
        _db.Boxers.Add(boxer);
        await _db.SaveChangesAsync();
        
        return boxer;
    }

    public async Task DeleteBoxer(Guid id)
    {
        var boxer = await _db.Boxers.FirstOrDefaultAsync(x => x.Id == id); ;

        if (boxer is null)
            return;
        
        _db.Boxers.Remove(boxer);
        await _db.SaveChangesAsync();

    }

    public async Task<IEnumerable<Boxer>> GetAllBoxers()
    {
        return await _db.Boxers.ToListAsync();
    }

    public async Task<Boxer?> GetBoxerById(Guid id)
    {
        return await _db.Boxers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Boxer?> UpdateBoxer(Guid id, BoxerUpdateDto dto)
    {
        var boxer = await _db.Boxers.FirstOrDefaultAsync(x => x.Id == id);
        
        if (boxer is not null)
        {
            if (dto.Age is not null) boxer.Age = dto.Age.Value;
            if (dto.WeightCategory is not null) boxer.WeightCategory = dto.WeightCategory;
            if (dto.FirstName is not null) boxer.FirstName = dto.FirstName;
            if (dto.LastName is not null) boxer.LastName = dto.LastName;
            await _db.SaveChangesAsync();
            return boxer;
        }
        return null;
    }
}
