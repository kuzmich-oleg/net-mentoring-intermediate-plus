using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.SeedData;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class SeatEntityConfiguration : IEntityTypeConfiguration<SeatEntity>
{
    public void Configure(EntityTypeBuilder<SeatEntity> builder)
    {
        builder.ToTable("Seats");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.SeatNumber)
            .IsRequired();

        builder
            .HasIndex(x => new { x.SectionRowId, x.SeatNumber })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder
            .HasOne(x => x.SectionRow)
            .WithMany(x => x.Seats)
            .HasForeignKey(x => x.SectionRowId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(Seats.DefaultSeats);
    }
}
