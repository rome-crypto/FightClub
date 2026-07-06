using FightClub.Entities.Fight;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FightClub.Configurations;

public class FightRoundConfiguration : IEntityTypeConfiguration<FightRound>
{
    public void Configure(EntityTypeBuilder<FightRound> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Number)
            .IsRequired();

        builder.HasMany(r => r.Events)
            .WithOne(e => e.FightRound)
            .HasForeignKey(e => e.FightRoundId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
