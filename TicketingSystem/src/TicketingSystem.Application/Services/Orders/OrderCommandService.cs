using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Application.Interfaces.Services;
using TicketingSystem.Application.Interfaces.Services.Commands;
using TicketingSystem.Application.Services.Orders.Models;
using TicketingSystem.Domain.Interfaces.Services;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Services.Orders;

internal sealed class OrderCommandService : IOrderCommandService
{
    private readonly ICartWriteRepository _cartWriteRepo;
    private readonly ICartReadRepository _cartReadRepo;
    private readonly IOfferReadRepository _offerReadRepo;
    private readonly IOfferWriteRepository _offerWriteRepo;
    private readonly IOrderWriteRepository _orderWriteRepo;
    private readonly IOrderReadRepository _orderReadRepo;

    private readonly ICurrentCustomerProvider _currentCustomerProvider;
    private readonly IOrdersService _ordersService;

    public OrderCommandService(
        ICartWriteRepository cartWriteRepo,
        ICartReadRepository cartReadRepo,
        IOfferReadRepository offerReadRepo,
        IOfferWriteRepository offerWriteRepo,
        IOrderWriteRepository orderWriteRepo,
        IOrderReadRepository orderReadRepo,
        ICurrentCustomerProvider currentCustomerProvider,
        IOrdersService ordersService)
    {
        _cartWriteRepo = cartWriteRepo;
        _cartReadRepo = cartReadRepo;
        _offerReadRepo = offerReadRepo;
        _offerWriteRepo = offerWriteRepo;
        _orderWriteRepo = orderWriteRepo;
        _orderReadRepo = orderReadRepo;
        _currentCustomerProvider = currentCustomerProvider;
        _ordersService = ordersService;
    }

    public async Task<Guid?> UpsertCartAsync(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var offer = await _offerReadRepo.GetByIdAsync(command.OfferId, cancellationToken);

        if (offer is null)
            return null;

        var cart = await _cartReadRepo.GetByIdAsync(command.CartId, cancellationToken);

        if (cart is null)
        {
            var cartModel = _ordersService.CreateCartFromOffer(command.CartId, offer,
                _currentCustomerProvider.CurrentCustomerId);

            var cartId = await _cartWriteRepo.AddAsync(cartModel, cancellationToken);

            return cartId;
        }

        if (cart.Status != CartStatus.Created || TryGetCartItem(cart, offer.EventId, offer.SeatId, out _))
            return null;

        var cartItem = new CartItem { CartId = cart.Id, OfferId = offer.Id };

        _ = await _cartWriteRepo.AddCartItemAsync(cartItem, cancellationToken);

        return command.CartId;
    }

    public async Task<Guid?> CreateOrderAsync(Guid cartId, CancellationToken cancellationToken)
    {
        var cart = await _cartReadRepo.GetByIdAsync(cartId, cancellationToken);

        var order = cart is null ? null : _ordersService.CreateOrder(cart);

        if (order is null)
            return null;

        var isCartUpdated = await _cartWriteRepo.UpdateAsync(cart!, cancellationToken);
        var seatStatus = cart!.Items
            .FirstOrDefault()?.Offer?.SeatStatus ?? SeatStatus.Booked;

        var areOffersUpdated = await _offerWriteRepo.UpdateSeatStatusAsync(
            [.. cart!.Items.Select(x => x.OfferId)],
            seatStatus,
            cancellationToken);

        if (!isCartUpdated || !areOffersUpdated)
            return null;

        _ = await _orderWriteRepo.AddAsync(order, cancellationToken);

        return order.Payments.FirstOrDefault()?.Id;
    }

    public async Task<bool> CompletePaymentAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepo.GetByPaymentIdAsync(paymentId, cancellationToken);

        if (!CanModifyOrder(order))
            return false;

        var completionResult = _ordersService.CompleteOrderPayment(order!, paymentId);

        if (!completionResult)
            return false;

        var updateResult = await UpdateOrderStatusAsync(completionResult, order!,
            SeatStatus.Available, cancellationToken);

        return updateResult;
    }

    public async Task<bool> RejectPaymentAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepo.GetByPaymentIdAsync(paymentId, cancellationToken);

        if (!CanModifyOrder(order))
            return false;

        var rejectionResult = _ordersService.RejectOrderPayment(order!, paymentId);

        var updateResult = await UpdateOrderStatusAsync(rejectionResult, order!,
            SeatStatus.Available, cancellationToken);

        return updateResult;
    }

    public async Task<bool> DeleteCartItemAsync(DeleteCartItemCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartReadRepo.GetByIdAsync(command.CartId, cancellationToken);

        if (cart is null || !TryGetCartItem(cart, command.EventId, command.SeatId, out var cartItem))
            return false;

        cartItem!.Offer!.SeatStatus = SeatStatus.Available;

        var isCartItemDeleted = await _cartWriteRepo.DeleteItemAsync(cartItem.Id, cancellationToken);

        if (!isCartItemDeleted)
            return false;

        _ = await _offerWriteRepo.UpdateAsync(cartItem.Offer, cancellationToken);

        return true;
    }

    private async Task<bool> UpdateOrderStatusAsync(bool isOperationCompeted, Order order,
        SeatStatus defaultSeatStatus, CancellationToken cancellationToken)
    {
        if (!isOperationCompeted)
            return false;

        var isOrderUpdated = await _orderWriteRepo.UpdateAsync(order!, cancellationToken);
        var seatStatus = order!.Cart!.Items
            .FirstOrDefault()?.Offer?.SeatStatus ?? defaultSeatStatus;

        var areOffersUpdated = await _offerWriteRepo.UpdateSeatStatusAsync(
            [.. order.Cart!.Items.Select(x => x.OfferId)],
            seatStatus,
            cancellationToken);

        return isOrderUpdated && areOffersUpdated;
    }

    private static bool TryGetCartItem(Cart cart, Guid eventId, Guid seatId, out CartItem? item)
    {
        item = cart.Items.FirstOrDefault(x =>
            x.Offer?.EventId == eventId
            && x.Offer.SeatId == seatId);

        return item is not null;
    }

    private static bool CanModifyOrder(Order? order)
        => order is not null;
}
