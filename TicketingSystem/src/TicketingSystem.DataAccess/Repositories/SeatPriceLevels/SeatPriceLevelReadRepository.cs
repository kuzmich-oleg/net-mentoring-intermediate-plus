using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.SeatPriceLevels;

internal sealed class SeatPriceLevelReadRepository : ISeatPriceLevelReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<SeatPriceLevelEntity> ActiveSeatPriceLevels
        => _dbContext.SeatPriceLevels.Where(x => !x.IsDeleted);

    public SeatPriceLevelReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SeatPriceLevelInfo?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var seatPriceLevelEntity = await ActiveSeatPriceLevels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var seatPriceLevelModel = seatPriceLevelEntity.MapIfNotNull(SeatPriceLevelMapper.FromEntity);
        return seatPriceLevelModel;
    }
}
