using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.EventManagers;

internal sealed class EventManagerReadRepository : IEventManagerReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<EventManagerEntity> ActiveEventManagers 
        => _dbContext.EventManagers.Where(x => !x.IsDeleted);

    public EventManagerReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EventManager?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var eventManagerEntity = await ActiveEventManagers
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var eventManagerModel = eventManagerEntity.MapIfNotNull(EventManagerMapper.FromEntity);
        return eventManagerModel;
    }

    public async Task<EventManager?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var eventManagerEntity = await ActiveEventManagers
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        var eventManagerModel = eventManagerEntity.MapIfNotNull(EventManagerMapper.FromEntity);
        return eventManagerModel;
    }
}
