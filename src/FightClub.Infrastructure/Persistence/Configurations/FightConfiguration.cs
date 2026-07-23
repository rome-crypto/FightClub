using FightClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FightClub.Infrastructure.Persistence.Configurations;

public sealed class FightConfiguration
    : IEntityTypeConfiguration<Fight>
{
    public void Configure(EntityTypeBuilder<Fight> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BoxerAId)
            .IsRequired();

        builder.Property(x => x.BoxerBId)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.EndType)
            .HasConversion<int>();

        builder.Property(x => x.PlannedRounds);

        builder.Property(x => x.WinnerId);

        builder.Navigation(x => x.Rounds)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
            .FindNavigation(nameof(Fight.Rounds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(f => f.Rounds, round =>
        {
            round.WithOwner();

            round.HasKey(x => x.Id);

            round.Property(x => x.Number);
            round.Property(x => x.ScoreA);
            round.Property(x => x.ScoreB);
            round.Property(x => x.IsFinished);



            round.OwnsMany(r => r.Events, e =>
            {
                e.WithOwner();

                e.HasKey(x => x.Id);

                e.Property(x => x.Type)
                    .HasConversion<int>();

                e.Property(x => x.BoxerId);

                e.Property(x => x.OccurredAt);
            });
        });
    }
}
