using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FightClub.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
