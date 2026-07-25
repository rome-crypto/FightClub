using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Common;

namespace FightClub.Domain.Entities.Auth;

public sealed class User : Entity
{
    private readonly List<UserRole> _roles = [];
    private readonly List<RefreshToken> _refreshTokens = [];

    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<UserRole> Roles => _roles;
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

    private User() { }

    public User(
        string userName,
        string email,
        string passwordHash)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
    }

    public void ChangePassword(string passwordHash)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User deactivated");
        }
        PasswordHash = passwordHash ?? string.Empty;
    }

    public void AddRole(Role role)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User deactivated");
        }
        _roles.Add(new UserRole(Id, role.Id));
    }

    public void RemoveRole(Guid roleId)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User deactivated");
        }
        UserRole role = _roles.Find(x => x.Id == roleId) 
            ?? throw new InvalidOperationException("Role not found");
        _roles.Remove(role);
    }

    public void AddRefreshToken(RefreshToken token)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User deactivated");
        }
        _refreshTokens.Add(token);
    }

    public void RevokeRefreshToken(Guid tokenId)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User deactivated");
        }
        RefreshToken token = _refreshTokens.Find(x => x.Id == tokenId)
            ?? throw new InvalidOperationException("Token not found");
        token.Revoke(DateTime.UtcNow);
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
