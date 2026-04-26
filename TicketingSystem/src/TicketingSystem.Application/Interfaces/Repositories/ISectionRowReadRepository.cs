using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ISectionRowReadRepository
{
    Task<SectionRow?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
