using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface IOrderReadRepository
{
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);

    Task<Order?> GetByCartIdAsync(Guid cartId, CancellationToken cancellationToken);

    Task<Order?> GetByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken);

    Task<IList<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
}
