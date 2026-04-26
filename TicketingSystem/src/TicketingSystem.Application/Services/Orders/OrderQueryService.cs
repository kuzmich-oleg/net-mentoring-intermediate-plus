using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Application.Interfaces.Services;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Services.Orders;

internal sealed class OrderQueryService : IOrderQueryService
{
    private readonly ICurrentCustomerProvider _currentCustomerProvider;
    private readonly ICartReadRepository _cartReadRepo;

    public OrderQueryService(
        ICurrentCustomerProvider currentCustomerProvider,
        ICartReadRepository cartReadRepo)
    {
        _currentCustomerProvider = currentCustomerProvider;
        _cartReadRepo = cartReadRepo;
    }

    public async Task<Cart?> GetCartAsync(Guid cartId, CancellationToken cancellationToken)
    {
        var cart = await _cartReadRepo.GetByIdAsync(cartId, cancellationToken);

        return cart != null && cart.CustomerId != _currentCustomerProvider.CurrentCustomerId
            ? throw new InvalidOperationException("Cart doen't belong to current customer")
            : cart;
    }
}
