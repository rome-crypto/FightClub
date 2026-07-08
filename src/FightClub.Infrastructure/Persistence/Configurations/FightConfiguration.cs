using FightClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FightClub.Infrastructure.Persistence.Configurations;

public class FightConfiguration : IEntityTypeConfiguration<Fight>
{
    public void Configure(EntityTypeBuilder<Fight> builder)
    {
        builder.HasKey(f => f.Id);

        builder.HasOne(f => f.BoxerA)
            .WithMany()
            .HasForeignKey(f => f.BoxerAId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.BoxerB)
            .WithMany()
            .HasForeignKey(f => f.BoxerBId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.Rounds)
            .WithOne(r => r.Fight)
            .HasForeignKey(r => r.FightId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
