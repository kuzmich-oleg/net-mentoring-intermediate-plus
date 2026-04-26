using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.EventManagers;

internal sealed class EventManagerWriteRepository : IEventManagerWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public EventManagerWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(EventManager eventManagerModel, CancellationToken cancellationToken)
    {
        var eventManagerEntity = EventManagerMapper.ToEntity(eventManagerModel);

        eventManagerEntity.Id = Guid.NewGuid();

        _dbContext.EventManagers.Add(eventManagerEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return eventManagerEntity.Id;
    }

    public async Task<bool> UpdateAsync(EventManager eventManagerModel, CancellationToken cancellationToken)
    {
        var eventManagerEntity = await GetByIdAsync(eventManagerModel.Id, cancellationToken);

        if (eventManagerEntity == null)
            return false;

        eventManagerEntity.FullName = eventManagerModel.FullName;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid eventManagerId, CancellationToken cancellationToken)
    {
        var eventManagerEntity = await GetByIdAsync(eventManagerId, cancellationToken);

        if (eventManagerEntity == null)
            return false;

        eventManagerEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<EventManagerEntity?> GetByIdAsync(Guid eventManagerId, CancellationToken cancellationToken)
    {
        return await _dbContext.EventManagers
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == eventManagerId, cancellationToken);
    }
}
