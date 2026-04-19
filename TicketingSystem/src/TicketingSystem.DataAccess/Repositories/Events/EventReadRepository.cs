using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Events;

internal sealed class EventReadRepository : IEventReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<EventEntity> ActiveEvents
        => _dbContext.Events.Where(x => !x.IsDeleted);

    public EventReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var eventEntity = await ActiveEvents
            .AsNoTracking()
            .Include(x => x.Venue)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var eventModel = eventEntity.MapIfNotNull(EventMapper.FromEntity);
        return eventModel;
    }

    public async Task<IReadOnlyCollection<Event>> GetEventsAsync(
        DateTimeOffset? eventDate,
        CancellationToken cancellationToken)
    {
        var query = ActiveEvents
            .AsNoTracking()
            .Include(x => x.Venue)
            .AsQueryable();

        if (eventDate.HasValue)
        {
            query = query.Where(x => x.EventDate.Date == eventDate.Value.Date);
        }

        var eventEntities = await query.ToListAsync(cancellationToken);

        var eventModels = eventEntities.MapToList(EventMapper.FromEntity);

        return eventModels;
    }
}
