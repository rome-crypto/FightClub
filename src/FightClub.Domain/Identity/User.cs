using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Common;
using FightClub.Domain.Enums;

namespace FightClub.Domain.Identity;

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

    public IReadOnlyCollection<string> RoleNames =>
        _roles.Select(x => x.Role.Name ?? string.Empty)
        .Where(n => !string.IsNullOrEmpty(n))
        .ToList();

    public bool HasRole(string roleName)
    {
        return _roles.Any(r => r.Role != null && r.Role.Name == roleName);
    }

    public bool HasRole(RoleType roleType)
    {
        return _roles.Any(r => r.Role != null && r.Role.Name == roleType.ToString());
    }

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

    public void AddRole(RoleType roleType)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User deactivated");
        }

        var roleName = roleType.ToString();

        if (_roles.Any(x => x.Role != null && x.Role.Name == roleName))
        {
            throw new InvalidOperationException($"Role {roleName} already assigned");
        }

        // Создаем роль (нужно будет загрузить из БД)
        var role = new Role(roleName);
        _roles.Add(new UserRole(Id, role));
    }

    public void AddRole(Role role)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User deactivated");
        }

        if (_roles.Any(x => x.RoleId == role.Id))
        {
            throw new InvalidOperationException("Role already assigned");
        }

        _roles.Add(new UserRole(Id, role));
    }

    public void AddRole(Guid roleId)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User deactivated");
        }

        _roles.Add(new UserRole(Id, roleId));
    }

    public void RemoveRole(Guid roleId)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("User deactivated");
        }
        UserRole userRole = _roles.Find(x => x.Id == roleId)
            ?? throw new InvalidOperationException("Role not found");
        _roles.Remove(userRole);
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
