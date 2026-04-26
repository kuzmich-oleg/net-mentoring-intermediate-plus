using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ICartWriteRepository
{
    Task<Guid> AddAsync(Cart cartModel, CancellationToken cancellationToken);
}
