using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Services;

public interface ICartCreationService
{
    Cart CreateFromOffer(Guid id, Offer offer, Guid customerId);
}
