using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Seats;

internal sealed class SeatWriteRepository : ISeatWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public SeatWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Seat seatModel, CancellationToken cancellationToken)
    {
        var seatEntity = SeatMapper.ToEntity(seatModel);

        seatEntity.Id = Guid.NewGuid();

        _dbContext.Seats.Add(seatEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return seatEntity.Id;
    }

    public async Task<bool> UpdateAsync(Seat seatModel, CancellationToken cancellationToken)
    {
        var seatEntity = await GetByIdAsync(seatModel.Id, cancellationToken);

        if (seatEntity == null)
            return false;

        seatEntity.SeatNumber = seatModel.SeatNumber;
        seatEntity.Type = seatModel.Type;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid seatId, CancellationToken cancellationToken)
    {
        var seatEntity = await GetByIdAsync(seatId, cancellationToken);

        if (seatEntity == null)
            return false;

        seatEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<SeatEntity?> GetByIdAsync(Guid seatId, CancellationToken cancellationToken)
    {
        return await _dbContext.Seats
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == seatId, cancellationToken);
    }
}
