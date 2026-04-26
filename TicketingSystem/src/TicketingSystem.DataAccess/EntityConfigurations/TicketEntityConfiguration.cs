using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class TicketEntityConfiguration : IEntityTypeConfiguration<TicketEntity>
{
    public void Configure(EntityTypeBuilder<TicketEntity> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(x => x.Id);

        builder
            .HasOne<CustomerEntity>()
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Offer)
            .WithOne(x => x.Ticket)
            .HasForeignKey<TicketEntity>(x => x.OfferId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Order)
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasIndex(x => new { x.CustomerId, x.OfferId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}
