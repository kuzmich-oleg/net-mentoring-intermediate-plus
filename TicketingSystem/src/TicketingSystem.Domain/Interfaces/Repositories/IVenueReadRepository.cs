using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface IVenueReadRepository
{
    Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Venue>> GetVenuesAsync(CancellationToken cancellationToken);
}
