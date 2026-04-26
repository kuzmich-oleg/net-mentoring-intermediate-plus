using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Services.Queries;

public interface IOrderQueryService
{
    Task<Cart?> GetCartAsync(Guid cartId, CancellationToken cancellationToken);
}
