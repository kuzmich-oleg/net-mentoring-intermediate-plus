using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Services;

public interface IOrdersService
{
    Cart CreateCartFromOffer(Guid id, Offer offer, Guid customerId);

    Order? CreateOrder(Cart cart);

    bool CompleteOrderPayment(Order order, Guid paymentId);

    bool RejectOrderPayment(Order order, Guid paymentId);
}
