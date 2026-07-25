using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Entities.Auth;

namespace FightClub.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);

    RefreshToken GenerateRefreshToken(User user);
}
