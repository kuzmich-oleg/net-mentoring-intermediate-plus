using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.EntityConfigurations;

internal sealed class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(DbConstants.ShortTextMaxLength);

        builder
            .Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(DbConstants.ShortTextMaxLength);

        builder
            .HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<CustomerEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
