using Microsoft.EntityFrameworkCore;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
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

        await _dbContext.Offers.AddAsync(offerEntity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return offerEntity.Id;
    }

    public async Task<bool> UpdateAsync(Offer offerModel, CancellationToken cancellationToken)
    {
        var offerEntity = await GetByIdAsync(offerModel.Id, cancellationToken);

        if (offerEntity == null)
            return false;

        offerEntity.Price = offerModel.Price;

        await _dbContext.SaveChangesAsync(cancellationToken);

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
