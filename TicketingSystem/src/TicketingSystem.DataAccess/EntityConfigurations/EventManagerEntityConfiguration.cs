using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class EventManagerEntityConfiguration : IEntityTypeConfiguration<EventManagerEntity>
{
    public void Configure(EntityTypeBuilder<EventManagerEntity> builder)
    {
        builder.ToTable("EventManagers");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(DbConstants.ShortTextMaxLength);

        builder
            .HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<EventManagerEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
