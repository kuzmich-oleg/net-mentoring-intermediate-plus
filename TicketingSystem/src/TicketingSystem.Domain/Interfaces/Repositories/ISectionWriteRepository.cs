using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface ISectionWriteRepository
{
    /// <summary>
    /// Adds a new section to the repository.
    /// </summary>
    /// <param name="sectionModel">The section model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added section.</returns>
    Task<Guid> AddAsync(Section sectionModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates Code only, other properties are not allowed to update.
    /// </summary>
    /// <param name="sectionModel">The section model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the section
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(Section sectionModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the section with the specified unique identifier.
    /// </summary>
    /// <param name="sectionId">The unique identifier of the section to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the section
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid sectionId, CancellationToken cancellationToken);
}
