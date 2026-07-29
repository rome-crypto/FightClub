using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Common;

namespace FightClub.Domain.Identity;

public sealed class RefreshToken : Entity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public string Token { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime ExpiresAt { get; private set; } 

    public DateTime? RevokedAt { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked => RevokedAt is not null;

    public bool IsActive => !IsExpired && !IsRevoked;

    private RefreshToken() { }

    public RefreshToken(
        Guid userId,
        string token,
        DateTime createdAt,
        DateTime expiresAt)
    {
        UserId = userId;
        Token = token;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }

    public void Revoke(DateTime revokedAt)
    {
        RevokedAt = revokedAt;
    }
}
