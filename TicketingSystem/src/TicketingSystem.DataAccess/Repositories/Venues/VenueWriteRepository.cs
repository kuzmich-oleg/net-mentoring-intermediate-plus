using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Venues;

internal sealed class VenueWriteRepository : IVenueWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public VenueWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Venue venueModel, CancellationToken cancellationToken)
    {
        var venueEntity = VenueMapper.ToEntity(venueModel);

        venueEntity.Id = Guid.NewGuid();

        _dbContext.Venues.Add(venueEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return venueEntity.Id;
    }

    public async Task<bool> UpdateAsync(Venue venueModel, CancellationToken cancellationToken)
    {
        var venueEntity = await GetByIdAsync(venueModel.Id, cancellationToken);

        if (venueEntity == null)
            return false;

        venueEntity.Name = venueModel.Name;
        venueEntity.Location = venueModel.Location;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid venueId, CancellationToken cancellationToken)
    {
        var venueEntity = await GetByIdAsync(venueId, cancellationToken);

        if (venueEntity == null)
            return false;

        venueEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<VenueEntity?> GetByIdAsync(Guid venueId, CancellationToken cancellationToken)
    {
        return await _dbContext.Venues
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == venueId, cancellationToken);
    }
}
