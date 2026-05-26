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

    private readonly ICurrentCustomerProvider _currentCustomerProvider;
    private readonly IOrdersService _ordersService;
    private readonly ICacheService _cacheService;

    public OrderCommandService(
        ICartWriteRepository cartWriteRepo,
        ICartReadRepository cartReadRepo,
        IOfferReadRepository offerReadRepo,
        IOfferWriteRepository offerWriteRepo,
        IOrderWriteRepository orderWriteRepo,
        ICurrentCustomerProvider currentCustomerProvider,
        IOrdersService ordersService,
        ICacheService cacheService)
    {
        _cartWriteRepo = cartWriteRepo;
        _cartReadRepo = cartReadRepo;
        _offerReadRepo = offerReadRepo;
        _offerWriteRepo = offerWriteRepo;
        _orderWriteRepo = orderWriteRepo;
        _currentCustomerProvider = currentCustomerProvider;
        _ordersService = ordersService;
        _cacheService = cacheService;
    }

    public async Task<Guid?> UpsertCartAsync(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var offer = await _offerReadRepo.GetByIdAsync(command.OfferId, cancellationToken);
        var isOfferInOtherCart = await _cartReadRepo.ExistAsync(command.OfferId, [CartStatus.OrderPlaced],
            cancellationToken);

        if (offer is null || isOfferInOtherCart)
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
            expectedCurrentStatus: SeatStatus.Available,
            seatStatus,
            cancellationToken);

        if (!isCartUpdated || !areOffersUpdated)
            return null;

        await RemoveSeatsRelatedCacheEntriesAsync(cart, cancellationToken);
        
        _ = await _orderWriteRepo.AddAsync(order, cancellationToken);

        return order.Payments.FirstOrDefault()?.Id;
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

    private async Task RemoveSeatsRelatedCacheEntriesAsync(Cart cart, CancellationToken cancellationToken)
    {
        var removeCacheEntryTasks = new List<Task>(cart.Items.Count);
        var eventSections = cart.Items
            .GroupBy(
                x => x.Offer?.EventId,
                v => v.Offer?.Seat?.SectionRow?.SectionId,
                (key, values) => (eventId: key, sectionsIds: values.Distinct()));

        foreach (var (eventId, sectionIds) in eventSections)
        {
            foreach (var sectionId in sectionIds)
            {
                if (eventId is null || sectionId is null)
                    continue;

                var offersCacheKey = CacheKeyHelper.GetSectionOffersCacheKey(eventId.Value, sectionId.Value);
                var removeCacheEntryTask = _cacheService.RemoveAsync(offersCacheKey, cancellationToken);

                removeCacheEntryTasks.Add(removeCacheEntryTask);
            }
        }

        await Task.WhenAll(removeCacheEntryTasks);
    }

    private static bool TryGetCartItem(Cart cart, Guid eventId, Guid seatId, out CartItem? item)
    {
        item = cart.Items.FirstOrDefault(x =>
            x.Offer?.EventId == eventId
            && x.Offer.SeatId == seatId);

        return item is not null;
    }
}
