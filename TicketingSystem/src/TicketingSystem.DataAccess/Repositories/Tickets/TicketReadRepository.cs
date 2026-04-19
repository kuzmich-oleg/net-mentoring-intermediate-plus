using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Tickets;

internal sealed class TicketReadRepository : ITicketReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<TicketEntity> ActiveTickets
        => _dbContext.Tickets.Where(x => !x.IsDeleted);

    public TicketReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var ticketEntity = await ActiveTickets
            .AsNoTracking()
            .Include(x => x.Offer)
                .ThenInclude(x => x!.Event)
                .ThenInclude(x => x!.Venue)
            .Include(x => x.Offer)
                .ThenInclude(x => x!.SeatPriceLevel)
            .Include(x => x.Offer)
                .ThenInclude(x => x!.Seat)
                .ThenInclude(x => x!.SectionRow)
                .ThenInclude(x => x!.Section)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var ticketModel = ticketEntity.MapIfNotNull(TicketMapper.FromEntity);
        return ticketModel;
    }
}
