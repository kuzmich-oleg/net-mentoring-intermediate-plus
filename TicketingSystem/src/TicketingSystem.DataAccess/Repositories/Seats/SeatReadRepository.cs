using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Seats;

internal sealed class SeatReadRepository : ISeatReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<SeatEntity> ActiveSeats
        => _dbContext.Seats.Where(x => !x.IsDeleted);

    public SeatReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Seat?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var seatEntity = await ActiveSeats
            .AsNoTracking()
            .Include(x => x.SectionRow)
                .ThenInclude(x => x!.Section)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var seatModel = seatEntity.MapIfNotNull(SeatMapper.FromEntity);
        return seatModel;
    }
}
