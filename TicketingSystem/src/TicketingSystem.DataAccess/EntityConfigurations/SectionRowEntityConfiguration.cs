using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class SectionRowEntityConfiguration : IEntityTypeConfiguration<SectionRowEntity>
{
    public void Configure(EntityTypeBuilder<SectionRowEntity> builder)
    {
        builder.ToTable("SectionRows");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(DbConstants.ShortTextMaxLength);

        builder
            .HasIndex(x => new { x.SectionId, x.Code })
            .IsUnique();

        builder
            .HasOne(x => x.Section)
            .WithMany(x => x.Rows)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
