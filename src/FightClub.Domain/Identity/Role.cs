using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Common;

namespace FightClub.Domain.Identity;

public sealed class Role : Entity
{
    private readonly List<RolePermission> _permissions = [];

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<RolePermission> Permissions => _permissions;

    private Role() { }

    public Role(string name)
    {
        Name = name;
    }

    public void AddPermission(Permission permission)
    {
        ArgumentNullException.ThrowIfNull(permission);
        _permissions.Add(new RolePermission(Id, permission.Id));
    }

    public void RemovePermission(Guid permissionId)
    {
        RolePermission permission = _permissions.Find(x => x.Id == permissionId) 
            ?? throw new Exception("Permission not found");

        _permissions.Remove(permission);
    }
}
