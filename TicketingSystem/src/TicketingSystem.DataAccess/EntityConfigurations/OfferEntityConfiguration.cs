using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class OfferEntityConfiguration : IEntityTypeConfiguration<OfferEntity>
{
    public void Configure(EntityTypeBuilder<OfferEntity> builder)
    {
        builder.ToTable("Offers");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder
            .HasOne(x => x.Event)
            .WithMany()
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Seat)
            .WithMany()
            .HasForeignKey(x => x.SeatId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.SeatPriceLevel)
            .WithMany()
            .HasForeignKey(x => x.SeatPriceLevelId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasIndex(x => new { x.SeatId, x.EventId, x.SeatPriceLevelId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}
