using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface ISeatPriceLevelReadRepository
{
    Task<SeatPriceLevelInfo?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
