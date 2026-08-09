using System.Security.Claims;
using FightClub.Domain.Identity;

namespace FightClub.Application.Common.Interfaces.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token);
}
