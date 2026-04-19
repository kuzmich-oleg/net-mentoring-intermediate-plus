using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Interfaces.Repositories;

public interface ICustomerWriteRepository
{
    /// <summary>
    /// Adds a new customer to the repository.
    /// </summary>
    /// <param name="customerModel">The customer model to add.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the add operation.</param>
    /// <returns>The ID of the newly added customer.</returns>
    Task<Guid> AddAsync(Customer customerModel, CancellationToken cancellationToken);

    /// <summary>
    /// Updates FirstName and LastName only, other properties are not allowed to update.
    /// </summary>
    /// <param name="customerModel">The customer model to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the customer
    /// was successfully updated; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(Customer customerModel, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the customer with the specified unique identifier.
    /// </summary>
    /// <param name="customerId">The unique identifier of the customer to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the customer
    /// was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteAsync(Guid customerId, CancellationToken cancellationToken);
}
