using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess;

public class TicketingDbContext(DbContextOptions<TicketingDbContext> options)
    : DbContext(options)
{
    public const string SchemaName = "Ticketing";

    internal virtual DbSet<UserEntity> Users => Set<UserEntity>();
    internal virtual DbSet<EventManagerEntity> EventManagers => Set<EventManagerEntity>();
    internal virtual DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
    internal virtual DbSet<EventEntity> Events => Set<EventEntity>();
    internal virtual DbSet<VenueEntity> Venues => Set<VenueEntity>();
    internal virtual DbSet<SectionEntity> Sections => Set<SectionEntity>();
    internal virtual DbSet<SectionRowEntity> SectionRows => Set<SectionRowEntity>();
    internal virtual DbSet<SeatEntity> Seats => Set<SeatEntity>();
    internal virtual DbSet<SeatPriceLevelEntity> SeatPriceLevels => Set<SeatPriceLevelEntity>();
    internal virtual DbSet<OfferEntity> Offers => Set<OfferEntity>();
    internal virtual DbSet<TicketEntity> Tickets => Set<TicketEntity>();
    internal virtual DbSet<CartEntity> Carts => Set<CartEntity>();
    internal virtual DbSet<CartItemEntity> CartItems => Set<CartItemEntity>();
    internal virtual DbSet<OrderEntity> Orders => Set<OrderEntity>();
    internal virtual DbSet<PaymentEntity> Payments => Set<PaymentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema(SchemaName)
            .ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    private void ApplyAuditInfo()
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var change in ChangeTracker.Entries())
        {
            if (change.Entity is not IAuditableEntity auditableEntity)
                continue;

            if (change.State == EntityState.Added)
                auditableEntity.CreatedAt = now;

            if (change.State == EntityState.Modified)
                auditableEntity.LastModifiedAt = now;
        }
    }
}
