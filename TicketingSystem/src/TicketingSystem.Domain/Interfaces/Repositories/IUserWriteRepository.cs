using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface IUserWriteRepository
{
    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="userModel">The user model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added user.</returns>
    Task<Guid> AddAsync(User userModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the User Type only, other properties are not allowed to update.
    /// </summary>
    /// <param name="userModel">The user model to update</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the user
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(User userModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the user with the specified unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the user
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid userId, CancellationToken cancellationToken);
}
