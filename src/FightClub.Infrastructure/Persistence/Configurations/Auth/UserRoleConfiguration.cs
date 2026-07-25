using FightClub.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FightClub.Infrastructure.Persistence.Configurations.Auth;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.RoleId)
            .IsRequired();

        // Почему составной индекс: быстрый поиск по связке UserId + RoleId
        // IsUnique: один пользователь не может иметь одну роль дважды
        builder.HasIndex(x => new { x.UserId, x.RoleId })
            .IsUnique();
    }
}
