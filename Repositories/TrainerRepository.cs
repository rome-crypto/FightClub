using FightClub.Data;
using FightClub.Entities;
using FightClub.Repositories.Interfaces;
namespace FightClub.Repositories;

public class TrainerRepository : Repository<Trainer>, ITrainerRepository
{
    public TrainerRepository(FightClubDbContext dbContext) : base(dbContext)
    {
    }
}
