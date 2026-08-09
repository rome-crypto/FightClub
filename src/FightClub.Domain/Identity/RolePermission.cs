using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Common;

namespace FightClub.Domain.Identity;

public sealed class RolePermission : Entity
{
    public Guid RoleId { get; private set; }

    public Guid PermissionId { get; private set; }

    private RolePermission() { }

    public RolePermission(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
}
