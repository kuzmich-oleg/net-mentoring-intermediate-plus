using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.SeedData;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class SectionEntityConfiguration : IEntityTypeConfiguration<SectionEntity>
{
    public void Configure(EntityTypeBuilder<SectionEntity> builder)
    {
        builder.ToTable("Sections");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(DbConstants.ShortTextMaxLength);

        builder
            .HasIndex(x => new { x.VenueId, x.Code })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder
            .HasOne(x => x.Venue)
            .WithMany(x => x.Sections)
            .HasForeignKey(x => x.VenueId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(Sections.DefaultSections);
    }
}
