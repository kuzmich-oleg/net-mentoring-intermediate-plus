using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Services.Venues;

internal sealed class VenueQueryService : IVenueQueryService
{
    private readonly IVenueReadRepository _venueReadRepo;

    public VenueQueryService(IVenueReadRepository venueReadRepo)
    {
        _venueReadRepo = venueReadRepo;
    }

    public async Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var venue = await _venueReadRepo.GetByIdAsync(id, cancellationToken);

        return venue;
    }

    public async Task<IReadOnlyCollection<Venue>> GetVenuesAsync(CancellationToken cancellationToken)
    {
        var venues = await _venueReadRepo.GetVenuesAsync(cancellationToken);

        return venues;
    }
}
