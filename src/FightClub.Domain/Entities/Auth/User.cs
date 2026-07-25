using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Common;

namespace FightClub.Domain.Entities.Auth;

public sealed class User : Entity
{
    private readonly List<UserRole> _roles = [];
    private readonly List<RefreshToken> _refreshTokens = [];

    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

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

    public void ChangePassword(string passwordHash);

    public void AddRole(Role role);

    public void RemoveRole(Guid roleId);

    public void AddRefreshToken(RefreshToken token);

    public void RevokeRefreshToken(Guid tokenId);

    public void Deactivate();
}
