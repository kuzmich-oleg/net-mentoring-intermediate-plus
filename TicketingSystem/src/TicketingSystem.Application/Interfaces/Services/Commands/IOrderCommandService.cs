using TicketingSystem.Application.Services.Orders.Models;

namespace TicketingSystem.Application.Interfaces.Services.Commands;

public interface IOrderCommandService
{
    Task<Guid?> CreateCartAsync(CreateCartCommand command, CancellationToken cancellationToken);
}
