using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface IEventWriteRepository
{
    /// <summary>
    /// Adds a new event to the repository.
    /// </summary>
    /// <param name="eventModel">The event model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added event.</returns>
    Task<Guid> AddAsync(Event eventModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates Name, Description, and EventDate only, other properties are not allowed to update.
    /// </summary>
    /// <param name="eventModel">The event model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the event
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(Event eventModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the event with the specified unique identifier.
    /// </summary>
    /// <param name="eventId">The unique identifier of the event to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the event
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid eventId, CancellationToken cancellationToken);
}
