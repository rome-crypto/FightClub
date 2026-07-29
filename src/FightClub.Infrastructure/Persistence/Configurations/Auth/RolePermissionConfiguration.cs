using FightClub.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FightClub.Infrastructure.Persistence.Configurations.Auth;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoleId)
            .IsRequired();

        builder.Property(x => x.PermissionId)
            .IsRequired();

        // Почему составной индекс IsUnique: одна роль не может иметь один пермишен дважды
        builder.HasIndex(x => new { x.RoleId, x.PermissionId })
            .IsUnique();
    }
}
