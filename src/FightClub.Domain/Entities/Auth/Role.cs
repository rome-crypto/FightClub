using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Common;

namespace FightClub.Domain.Entities.Auth;

public sealed class Role : Entity
{
    private readonly List<RolePermission> _permissions = [];

    public string Name { get; private set; }

    public IReadOnlyCollection<RolePermission> Permissions => _permissions;

    private Role() { }

    public Role(string name)
    {
        Name = name;
    }

    public void AddPermission(Permission permission);

    public void RemovePermission(Guid permissionId);
}
