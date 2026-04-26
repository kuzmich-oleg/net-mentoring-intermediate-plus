using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface IVenueWriteRepository
{
    /// <summary>
    /// Adds a new venue to the repository.
    /// </summary>
    /// <param name="venueModel">The venue model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added venue.</returns>
    Task<Guid> AddAsync(Venue venueModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates Name and Location only, other properties are not allowed to update.
    /// </summary>
    /// <param name="venueModel">The venue model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the venue
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(Venue venueModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the venue with the specified unique identifier.
    /// </summary>
    /// <param name="venueId">The unique identifier of the venue to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the venue
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid venueId, CancellationToken cancellationToken);
}
