using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Services.Queries;

public interface IVenueQueryService
{
    Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Venue>> GetVenuesAsync(CancellationToken cancellationToken);
}
