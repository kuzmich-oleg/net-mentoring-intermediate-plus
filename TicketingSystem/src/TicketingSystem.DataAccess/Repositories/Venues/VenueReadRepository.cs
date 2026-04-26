using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Venues;

internal sealed class VenueReadRepository : IVenueReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<VenueEntity> ActiveVenues
        => _dbContext.Venues.Where(x => !x.IsDeleted);

    public VenueReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var venueEntity = await ActiveVenues
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Sections.Where(x => !x.IsDeleted))
            .ThenInclude(x => x.Rows.Where(x => !x.IsDeleted))
            .ThenInclude(x => x.Seats.Where(x => !x.IsDeleted))
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var venueModel = venueEntity.MapIfNotNull(VenueMapper.FromEntity);
        return venueModel;
    }

    public async Task<IReadOnlyCollection<Venue>> GetVenuesAsync(CancellationToken cancellationToken)
    {
        var venueEntity = await ActiveVenues
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var venueModels = venueEntity.MapToList(VenueMapper.FromEntity);
        return venueModels;
    }
}
