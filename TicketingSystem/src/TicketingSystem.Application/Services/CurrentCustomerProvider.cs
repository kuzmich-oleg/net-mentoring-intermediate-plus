using TicketingSystem.Application.Interfaces.Services;

namespace TicketingSystem.Application.Services;

internal sealed class CurrentCustomerProvider : ICurrentCustomerProvider
{
    // In a real application, this would likely be implemented using the current user's claims
    public Guid CurrentCustomerId => Guid.Parse("391e388c-57a0-85b8-bf75-019dca57d1f5");
}
