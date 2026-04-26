using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Events;

internal sealed class EventWriteRepository : IEventWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public EventWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Event eventModel, CancellationToken cancellationToken)
    {
        var eventEntity = EventMapper.ToEntity(eventModel);

        eventEntity.Id = Guid.NewGuid();

        _dbContext.Events.Add(eventEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return eventEntity.Id;
    }

    public async Task<bool> UpdateAsync(Event eventModel, CancellationToken cancellationToken)
    {
        var eventEntity = await GetByIdAsync(eventModel.Id, cancellationToken);

        if (eventEntity == null)
            return false;

        eventEntity.Name = eventModel.Name;
        eventEntity.Description = eventModel.Description;
        eventEntity.EventDate = eventModel.EventDate;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var eventEntity = await GetByIdAsync(eventId, cancellationToken);

        if (eventEntity == null)
            return false;

        eventEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<EventEntity?> GetByIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await _dbContext.Events
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == eventId, cancellationToken);
    }
}
