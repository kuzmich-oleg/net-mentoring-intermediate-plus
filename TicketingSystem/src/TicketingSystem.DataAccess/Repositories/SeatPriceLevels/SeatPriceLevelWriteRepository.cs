using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.SeatPriceLevels;

internal sealed class SeatPriceLevelWriteRepository : ISeatPriceLevelWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public SeatPriceLevelWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(SeatPriceLevelInfo seatPriceLevelModel, CancellationToken cancellationToken)
    {
        var seatPriceLevelEntity = SeatPriceLevelMapper.ToEntity(seatPriceLevelModel);

        seatPriceLevelEntity.Id = Guid.NewGuid();

        _dbContext.SeatPriceLevels.Add(seatPriceLevelEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return seatPriceLevelEntity.Id;
    }

    public async Task<bool> UpdateAsync(SeatPriceLevelInfo seatPriceLevelModel, CancellationToken cancellationToken)
    {
        var seatPriceLevelEntity = await GetByIdAsync(seatPriceLevelModel.Id, cancellationToken);

        if (seatPriceLevelEntity == null)
            return false;

        seatPriceLevelEntity.PriceLevel = seatPriceLevelModel.PriceLevel;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid seatPriceLevelId, CancellationToken cancellationToken)
    {
        var seatPriceLevelEntity = await GetByIdAsync(seatPriceLevelId, cancellationToken);

        if (seatPriceLevelEntity == null)
            return false;

        seatPriceLevelEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<SeatPriceLevelEntity?> GetByIdAsync(Guid seatPriceLevelId, CancellationToken cancellationToken)
    {
        return await _dbContext.SeatPriceLevels
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == seatPriceLevelId, cancellationToken);
    }
}
