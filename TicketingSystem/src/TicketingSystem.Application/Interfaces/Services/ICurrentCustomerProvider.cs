namespace TicketingSystem.Application.Interfaces.Services;

public interface ICurrentCustomerProvider
{
    Guid CurrentCustomerId { get; }
}
