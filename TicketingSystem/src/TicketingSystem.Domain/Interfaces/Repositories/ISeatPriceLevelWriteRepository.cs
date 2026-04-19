using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface ISeatPriceLevelWriteRepository
{
    /// <summary>
    /// Adds a new seat price level to the repository.
    /// </summary>
    /// <param name="seatPriceLevelModel">The seat price level model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added seat price level.</returns>
    Task<Guid> AddAsync(SeatPriceLevelInfo seatPriceLevelModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates PriceLevel only, other properties are not allowed to update.
    /// </summary>
    /// <param name="seatPriceLevelModel">The seat price level model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the seat price level
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(SeatPriceLevelInfo seatPriceLevelModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the seat price level with the specified unique identifier.
    /// </summary>
    /// <param name="seatPriceLevelId">The unique identifier of the seat price level to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the seat price level
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid seatPriceLevelId, CancellationToken cancellationToken);
}
