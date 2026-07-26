using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Common;

namespace FightClub.Domain.Entities.Auth;

public sealed class UserRole : Entity
{
    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }


    public Role Role { get; private set; } = null!;

    private UserRole() { }

    public UserRole(Guid userId, Role role)
    {
        UserId = userId;
        RoleId = role.Id;
        Role = role;
    }

    public UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}
