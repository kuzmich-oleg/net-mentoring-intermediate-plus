using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Tickets;

internal sealed class TicketWriteRepository : ITicketWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public TicketWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Ticket ticketModel, CancellationToken cancellationToken)
    {
        var ticketEntity = TicketMapper.ToEntity(ticketModel);

        ticketEntity.Id = Guid.NewGuid();

        await _dbContext.Tickets.AddAsync(ticketEntity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ticketEntity.Id;
    }

    public async Task<bool> DeleteAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        var ticketEntity = await GetByIdAsync(ticketId, cancellationToken);

        if (ticketEntity == null)
            return false;

        ticketEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<TicketEntity?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        return await _dbContext.Tickets
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == ticketId, cancellationToken);
    }
}
