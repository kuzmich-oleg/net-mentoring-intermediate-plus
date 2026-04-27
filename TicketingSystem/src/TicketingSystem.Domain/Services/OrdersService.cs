using TicketingSystem.Domain.Interfaces.Services;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Services;

internal sealed class OrdersService : IOrdersService
{
    private readonly Dictionary<OrderStatus, (PaymentStatus payment, SeatStatus seat)> _statusMap = new()
    {
        [OrderStatus.Paid] = (PaymentStatus.Completed, SeatStatus.Sold),
        [OrderStatus.Canceled] = (PaymentStatus.Failed , SeatStatus.Available)
    };

    // for now it just creates a cart, but it also should validate the offer, provided cart id, update related seats availability and so on.
    // so it is better to keep this logic in a separate service, rather than in the application layer.
    public Cart CreateCartFromOffer(Guid id, Offer offer, Guid customerId)
    {
        //if (offer.SeatStatus != SeatStatus.Available)
        //    throw new InvalidOperationException("Offer is not available.");

        return new Cart
        {
            Id = id,
            CustomerId = customerId,
            Status = CartStatus.Created,
            Items = [ new() { CartId = id, OfferId = offer.Id } ]
        };
    }

    public Order? CreateOrder(Cart cart)
    {
        if (cart.Items.Count == 0)
            return null;

        cart.Status = CartStatus.OrderPlaced;

        foreach (var item in cart.Items)
            item.Offer!.SeatStatus = SeatStatus.Booked;

        var order = CreateOrderInternal(cart);

        var payment = CreatePaymentInternal(cart, order.Id);

        order.Payments.Add(payment);

        return order;
    }

    public bool CompleteOrderPayment(Order order, Guid paymentId)
        => SetOrderPaymentStatus(order, paymentId, OrderStatus.Paid);

    public bool RejectOrderPayment(Order order, Guid paymentId)
        => SetOrderPaymentStatus(order, paymentId, OrderStatus.Canceled);

    //semi mock implementation
    private bool SetOrderPaymentStatus(Order order, Guid paymentId, OrderStatus orderStatus)
    {
        var payment = order.Payments.FirstOrDefault(p => p.Id == paymentId);
        var isStatusValid = _statusMap.TryGetValue(orderStatus, out var subStatuses);

        if (payment is null || !CanModifyOrder(order) || !isStatusValid)
            return false;

        foreach (var item in order.Cart!.Items)
            item.Offer!.SeatStatus = subStatuses.seat;

        order.Status = orderStatus;
        payment.Status = subStatuses.payment;

        return true;
    }

    private static bool CanModifyOrder(Order? order)
        => order is not null
            && order.Cart is not null
            && order.Cart.Items.Count > 0
            && order.Status == OrderStatus.Placed;

    private static Order CreateOrderInternal(Cart cart)
        => new()
        {
            Id = Guid.NewGuid(),
            CustomerId = cart.CustomerId,
            CartId = cart.Id,
            Status = OrderStatus.Placed,
            Payments = [],
            CreatedAt = DateTime.UtcNow
        };

    private static Payment CreatePaymentInternal(Cart cart, Guid orderId)
        => new()
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ExternalId = Guid.NewGuid(), // in real life it should be provided by payment provider
            Status = PaymentStatus.Pending,
            Amount = cart.TotalPrice,
            CreatedAt = DateTime.UtcNow
        };
}
