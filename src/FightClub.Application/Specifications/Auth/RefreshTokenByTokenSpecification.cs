using FightClub.Application.Specifications.Common;
using FightClub.Domain.Identity;

namespace FightClub.Application.Specifications.Auth;

public sealed class RefreshTokenByTokenSpecification : BaseSpecification<RefreshToken>
{
    public RefreshTokenByTokenSpecification(string token)
    {
        // WHERE Token = @token
        AddCriteria(rt => rt.Token == token);
    }
}
