using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Entities.Abstractions;

namespace TicketingSystem.DataAccess;

public sealed class TicketingDbContext(DbContextOptions<TicketingDbContext> options)
    : DbContext(options)
{
    public const string SchemaName = "Ticketing";

    internal DbSet<UserEntity> Users => Set<UserEntity>();
    internal DbSet<EventManagerEntity> EventManagers => Set<EventManagerEntity>();
    internal DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
    internal DbSet<EventEntity> Events => Set<EventEntity>();
    internal DbSet<VenueEntity> Venues => Set<VenueEntity>();
    internal DbSet<SectionEntity> Sections => Set<SectionEntity>();
    internal DbSet<SectionRowEntity> SectionRows => Set<SectionRowEntity>();
    internal DbSet<SeatEntity> Seats => Set<SeatEntity>();
    internal DbSet<SeatPriceLevelEntity> SeatPriceLevels => Set<SeatPriceLevelEntity>();
    internal DbSet<OfferEntity> Offers => Set<OfferEntity>();
    internal DbSet<TicketEntity> Tickets => Set<TicketEntity>();

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
