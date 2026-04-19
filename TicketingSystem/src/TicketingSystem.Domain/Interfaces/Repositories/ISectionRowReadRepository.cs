using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface ISectionRowReadRepository
{
    Task<SectionRow?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
