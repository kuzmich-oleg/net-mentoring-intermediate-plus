using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class VenueEntityConfiguration : IEntityTypeConfiguration<VenueEntity>
{
    public void Configure(EntityTypeBuilder<VenueEntity> builder)
    {
        builder.ToTable("Venues");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(DbConstants.ShortTextMaxLength);

        builder
            .Property(x => x.Location)
            .IsRequired()
            .HasMaxLength(DbConstants.ShortTextMaxLength);
    }
}
