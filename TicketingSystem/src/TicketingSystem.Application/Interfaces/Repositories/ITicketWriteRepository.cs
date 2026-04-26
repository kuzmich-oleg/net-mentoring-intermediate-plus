using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ITicketWriteRepository
{
    /// <summary>
    /// Adds a new ticket to the repository.
    /// </summary>
    /// <param name="ticketModel">The ticket model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added ticket.</returns>
    Task<Guid> AddAsync(Ticket ticketModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the ticket with the specified unique identifier.
    /// </summary>
    /// <param name="ticketId">The unique identifier of the ticket to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the ticket
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid ticketId, CancellationToken cancellationToken);
}
