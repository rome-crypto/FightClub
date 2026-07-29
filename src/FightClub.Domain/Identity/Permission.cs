using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FightClub.Domain.Common;

namespace FightClub.Domain.Identity;

public sealed class Permission : Entity
{
    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    private Permission() { }

    public Permission(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
