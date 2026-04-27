using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Offers;

internal sealed class OfferWriteRepository : IOfferWriteRepository
{
    private readonly TicketingDbContext _dbContext;

    public OfferWriteRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Offer offerModel, CancellationToken cancellationToken)
    {
        var offerEntity = OfferMapper.ToEntity(offerModel);

        offerEntity.Id = Guid.NewGuid();

        _dbContext.Offers.Add(offerEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return offerEntity.Id;
    }

    public async Task<bool> UpdateAsync(Offer offerModel, CancellationToken cancellationToken)
    {
        var offerEntity = await GetByIdAsync(offerModel.Id, cancellationToken);

        if (offerEntity == null)
            return false;

        offerEntity.Price = offerModel.Price;
        offerEntity.SeatStatus = offerModel.SeatStatus;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> UpdateSeatStatusAsync(Guid[] offerIds, SeatStatus seatStatus,
        CancellationToken cancellationToken)
    {
        if (offerIds == null || offerIds.Length == 0)
            return false;

        var offerEntities = await _dbContext.Offers
            .Where(x => offerIds.Contains(x.Id) && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        if (offerEntities.Count != offerIds.Length)
            return false;

        await _dbContext.Offers
            .Where(x => offerIds.Contains(x.Id) && !x.IsDeleted)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.SeatStatus, seatStatus));

        return true;
    }

    public async Task<bool> DeleteAsync(Guid offerId, CancellationToken cancellationToken)
    {
        var offerEntity = await GetByIdAsync(offerId, cancellationToken);

        if (offerEntity == null)
            return false;

        offerEntity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<OfferEntity?> GetByIdAsync(Guid offerId, CancellationToken cancellationToken)
    {
        return await _dbContext.Offers
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == offerId, cancellationToken);
    }
}
