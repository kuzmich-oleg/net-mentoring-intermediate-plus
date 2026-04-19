using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface IEventManagerWriteRepository
{
    /// <summary>
    /// Adds a new event manager to the repository.
    /// </summary>
    /// <param name="eventManagerModel">The event manager model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added event manager.</returns>
    Task<Guid> AddAsync(EventManager eventManagerModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the FullName only, other properties are not allowed to update.
    /// </summary>
    /// <param name="eventManagerModel">The event manager model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the event manager
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(EventManager eventManagerModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the event manager with the specified unique identifier.
    /// </summary>
    /// <param name="eventManagerId">The unique identifier of the event manager to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the event manager
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid eventManagerId, CancellationToken cancellationToken);
}
