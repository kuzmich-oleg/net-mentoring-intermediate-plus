using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class EventEntityConfiguration : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(DbConstants.ShortTextMaxLength);

        builder
            .Property(x => x.Description)
            .IsRequired();

        builder
            .Property(x => x.EventDate)
            .IsRequired();

        builder
            .HasOne(x => x.Venue)
            .WithMany()
            .HasForeignKey(x => x.VenueId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
