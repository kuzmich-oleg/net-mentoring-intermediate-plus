using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ICartWriteRepository
{
    Task<Guid> AddAsync(Cart cartModel, CancellationToken cancellationToken);

    Task<Guid> AddCartItemAsync(CartItem cartItemModel, CancellationToken cancellationToken);

    Task<bool> UpdateAsync(Cart cartModel, CancellationToken cancellationToken);

    Task<bool> DeleteItemAsync(Guid cartItemId, CancellationToken cancellationToken);
}
