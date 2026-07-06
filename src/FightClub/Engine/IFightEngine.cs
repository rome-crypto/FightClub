using FightClub.Entities.Fight;

namespace FightClub.Engine;

public interface IFightEngine
{
    Fight Execute(Fight fight);
}
