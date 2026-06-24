using FightClub.Entities;
namespace FightClub.Repositories.Interfaces;

public interface IBoxerRepository : IRepository<Boxer>
{
    Task<List<Boxer>> GetByWeightCategoryAsync(string weightCategory);
    Task<List<Boxer>> GetByAgeAsync(int age);
}
