using FightClub.Entities.Fight;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FightClub.Configurations;

public class RoundEventConfiguration : IEntityTypeConfiguration<RoundEvent>
{
    public void Configure(EntityTypeBuilder<RoundEvent> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Type)
            .IsRequired();

        builder.Property(e => e.Impact)
            .IsRequired();
    }
}
