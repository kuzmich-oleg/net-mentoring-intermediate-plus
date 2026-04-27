using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface IOrderWriteRepository
{
    Task<Guid> AddAsync(Order orderModel, CancellationToken cancellationToken);

    Task<bool> UpdateAsync(Order orderModel, CancellationToken cancellationToken);
}
