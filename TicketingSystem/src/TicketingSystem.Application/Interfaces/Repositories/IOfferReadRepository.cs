using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface IOfferReadRepository
{
    Task<Offer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Offer>> GetEventOffersAsync(Guid eventId, Guid? sectionId, CancellationToken cancellationToken);
}
