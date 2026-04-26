using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ISectionRowWriteRepository
{
    /// <summary>
    /// Adds a new section row to the repository.
    /// </summary>
    /// <param name="sectionRowModel">The section row model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added section row.</returns>
    Task<Guid> AddAsync(SectionRow sectionRowModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates Code only, other properties are not allowed to update.
    /// </summary>
    /// <param name="sectionRowModel">The section row model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the section row
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(SectionRow sectionRowModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the section row with the specified unique identifier.
    /// </summary>
    /// <param name="sectionRowId">The unique identifier of the section row to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the section row
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid sectionRowId, CancellationToken cancellationToken);
}
