using FightClub.Data;
using FightClub.Entities.Fight;
using FightClub.Repositories.Interfaces;
using FightClub.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Repositories;

public class FightRepository : Repository<Fight>, IFightRepository
{
    public FightRepository(FightClubDbContext dbContext) : base(dbContext)
    {
    }
}
