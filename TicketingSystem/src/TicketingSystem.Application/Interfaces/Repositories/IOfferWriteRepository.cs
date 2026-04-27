using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Interfaces.Repositories;

public interface IOfferWriteRepository
{
    /// <summary>
    /// Adds a new offer to the repository.
    /// </summary>
    /// <param name="offerModel">The offer model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added offer.</returns>
    Task<Guid> AddAsync(Offer offerModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates Price only, other properties are not allowed to update.
    /// </summary>
    /// <param name="offerModel">The offer model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the offer
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(Offer offerModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously updates the status of seats associated with the specified offer identifiers.
    /// </summary>
    /// <param name="offerIds">An array of offer identifiers for which the seat status will be updated. Cannot be null or empty.</param>
    /// <param name="seatStatus">The new status to apply to the seats associated with the specified offers.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the seat statuses were
    /// successfully updated for all specified offers; otherwise, false.</returns>
    Task<bool> UpdateSeatStatusAsync(Guid[] offerIds, SeatStatus seatStatus, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the offer with the specified unique identifier.
    /// </summary>
    /// <param name="offerId">The unique identifier of the offer to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the offer
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid offerId, CancellationToken cancellationToken);
}
