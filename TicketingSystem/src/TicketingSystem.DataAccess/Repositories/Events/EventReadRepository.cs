using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Common;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Extensions;
using TicketingSystem.DataAccess.Mappers;
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
            .AsSplitQuery()
            .Include(x => x.Venue)
                .ThenInclude(x => x!.Sections.Where(x => !x.IsDeleted))
            .Where(x => !x.Venue!.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var eventModel = eventEntity.MapIfNotNull(EventMapper.FromEntity);
        return eventModel;
    }

    public async Task<PagedResult<Event>> GetEventsAsync(
        string? namePart,
        DateTimeOffset? eventDate,
        OffsetPage offsetPage,
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

        if (!string.IsNullOrEmpty(namePart))
        {
            var pattern = $"%{namePart}%";
            query = query.Where(x => EF.Functions.Like(x.Name, pattern));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var eventEntities = await query
            .OrderBy(x => x.EventDate)
            .ApplyPaging(offsetPage)
            .ToListAsync(cancellationToken);

        var eventModels = eventEntities.MapToList(EventMapper.FromEntity);

        return new PagedResult<Event>(totalCount, offsetPage, eventModels);
    }
}
