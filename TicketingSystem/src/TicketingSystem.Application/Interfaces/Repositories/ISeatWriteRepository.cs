using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface ISeatWriteRepository
{
    /// <summary>
    /// Adds a new seat to the repository.
    /// </summary>
    /// <param name="seatModel">The seat model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added seat.</returns>
    Task<Guid> AddAsync(Seat seatModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates SeatNumber and Type only, other properties are not allowed to update.
    /// </summary>
    /// <param name="seatModel">The seat model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the seat
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(Seat seatModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the seat with the specified unique identifier.
    /// </summary>
    /// <param name="seatId">The unique identifier of the seat to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the seat
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid seatId, CancellationToken cancellationToken);
}
