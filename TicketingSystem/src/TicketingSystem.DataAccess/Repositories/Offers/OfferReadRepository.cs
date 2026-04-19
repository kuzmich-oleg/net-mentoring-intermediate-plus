using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Domain.Interfaces.Repositories;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.Repositories.Offers;

internal sealed class OfferReadRepository : IOfferReadRepository
{
    private readonly TicketingDbContext _dbContext;

    private IQueryable<OfferEntity> ActiveOffers
        => _dbContext.Offers.Where(x => !x.IsDeleted);

    public OfferReadRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Offer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var offerEntity = await ActiveOffers
            .AsNoTracking()
            .Include(x => x.Seat)
                .ThenInclude(x => x!.SectionRow)
                .ThenInclude(x => x!.Section)
            .Include(x => x.SeatPriceLevel)
            .Include(x => x.Event)
                .ThenInclude(x => x!.Venue)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var offerModel = offerEntity.MapIfNotNull(OfferMapper.FromEntity);
        return offerModel;
    }
}
