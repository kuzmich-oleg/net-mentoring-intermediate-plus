using TicketingSystem.Application.Services.Orders.Models;

namespace TicketingSystem.Application.Interfaces.Services.Commands;

// TODO: in real life it should be split into several services, for example CartCommandService, OrderCommandService, PaymentCommandService and so on,
// but for the sake of simplicity we will keep it in one service for now.

/// <summary>
/// Service that manages order related commands, such as creating/updating cart, placing order and payment and so on.
/// </summary>
public interface IOrderCommandService
{
    Task<Guid?> UpsertCartAsync(CreateCartCommand command, CancellationToken cancellationToken);

    Task<Guid?> CreateOrderAsync(Guid cartId, CancellationToken cancellationToken);

    Task<bool> CompletePaymentAsync(Guid paymentId, CancellationToken cancellationToken);

    Task<bool> RejectPaymentAsync(Guid paymentId, CancellationToken cancellationToken);

    Task<bool> DeleteCartItemAsync(DeleteCartItemCommand command, CancellationToken cancellationToken);
}
