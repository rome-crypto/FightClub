using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FightClub.Entities;

public class BoxerConfiguration : IEntityTypeConfiguration<Boxer>
{
    public void Configure(EntityTypeBuilder<Boxer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Age)
            .IsRequired();

        builder.Property(x => x.WeightCategory)
            .HasMaxLength(30);

        builder.HasOne(x => x.Trainer)
            .WithMany(t => t.Boxers)
            .HasForeignKey(x => x.TrainerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
