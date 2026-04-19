using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface IOfferReadRepository
{
    Task<Offer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
