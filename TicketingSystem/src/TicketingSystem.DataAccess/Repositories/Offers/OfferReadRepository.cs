using Microsoft.EntityFrameworkCore;
using TicketingSystem.Common.Extensions;
using TicketingSystem.DataAccess.Entities;
using TicketingSystem.DataAccess.Mappers;
using TicketingSystem.Application.Interfaces.Repositories;
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
            .AsSplitQuery()
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

    public async Task<IReadOnlyCollection<Offer>> GetEventOffersAsync(Guid eventId, Guid? sectionId,
        CancellationToken cancellationToken)
    {
        var query = ActiveOffers
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Seat)
                .ThenInclude(x => x!.SectionRow)
                .ThenInclude(x => x!.Section)
            .Include(x => x.SeatPriceLevel)
            .Where(x => x.EventId == eventId);

        if (sectionId.HasValue)
        {
            query = query.Where(x => x.Seat!.SectionRow!.SectionId == sectionId.Value);
        }

        var offerEntities = await query.ToListAsync(cancellationToken);
        var offerModels = offerEntities.MapToList(OfferMapper.FromEntity);

        return offerModels;
    }
}
