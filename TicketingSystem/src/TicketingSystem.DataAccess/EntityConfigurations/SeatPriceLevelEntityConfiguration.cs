using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class SeatPriceLevelEntityConfiguration : IEntityTypeConfiguration<SeatPriceLevelEntity>
{
    public void Configure(EntityTypeBuilder<SeatPriceLevelEntity> builder)
    {
        builder.ToTable("SeatPriceLevels");

        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => x.PriceLevel)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}
