using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Application.Interfaces.Services;
using TicketingSystem.Application.Interfaces.Services.Commands;
using TicketingSystem.Application.Services.Orders.Models;
using TicketingSystem.Domain.Interfaces.Services;

namespace TicketingSystem.Application.Services.Orders;

internal sealed class OrderCommandService : IOrderCommandService
{
    private readonly ICartWriteRepository _cartWriteRepo;
    private readonly ICartReadRepository _cartReadRepo;
    private readonly IOfferReadRepository _offerReadRepo;
    private readonly ICurrentCustomerProvider _currentCustomerProvider;
    private readonly ICartCreationService _cartCreationService;

    public OrderCommandService(
        ICartWriteRepository cartWriteRepo,
        ICartReadRepository cartReadRepo,
        IOfferReadRepository offerReadRepo,
        ICurrentCustomerProvider currentCustomerProvider,
        ICartCreationService cartCreationService)
    {
        _cartWriteRepo = cartWriteRepo;
        _cartReadRepo = cartReadRepo;
        _offerReadRepo = offerReadRepo;
        _currentCustomerProvider = currentCustomerProvider;
        _cartCreationService = cartCreationService;
    }

    public async Task<Guid?> CreateCartAsync(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var offer = await _offerReadRepo.GetByIdAsync(command.OfferId, cancellationToken);
        var cartIdIsUsed = await _cartReadRepo.ExistsAsync(command.CartId, cancellationToken);

        if (offer is null || cartIdIsUsed)
            return null;

        var cartModel = _cartCreationService.CreateFromOffer(command.CartId, offer,
            _currentCustomerProvider.CurrentCustomerId);

        var cartId = await _cartWriteRepo.AddAsync(cartModel, cancellationToken);
        return cartId;
    }
}
