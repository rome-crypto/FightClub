using FightClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FightClub.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Age)
            .IsRequired();

        // Trainer → Boxers
        builder.HasMany(x => x.Boxers)
            .WithOne(b => b.Trainer)
            .HasForeignKey(b => b.TrainerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
