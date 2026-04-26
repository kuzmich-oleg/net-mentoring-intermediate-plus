using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ISeatPriceLevelReadRepository
{
    Task<SeatPriceLevelInfo?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
