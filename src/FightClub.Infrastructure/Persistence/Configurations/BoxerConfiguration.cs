using FightClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FightClub.Infrastructure.Persistence.Configurations;

public sealed class BoxerConfiguration
    : IEntityTypeConfiguration<Boxer>
{
    public void Configure(EntityTypeBuilder<Boxer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.DateOfBirth)
            .IsRequired();

        builder.Property(x => x.Weight)
            .IsRequired();

        builder.Property(x => x.TrainerId);

        builder.OwnsOne(x => x.Statistics);

        builder.OwnsOne(x => x.Ranking);
    }
}
