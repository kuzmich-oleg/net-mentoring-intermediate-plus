namespace TicketingSystem.Application.Interfaces.Services.Commands;

public interface IPaymentCommandService
{
    Task<bool> CompletePaymentAsync(Guid paymentId, CancellationToken cancellationToken);

    Task<bool> RejectPaymentAsync(Guid paymentId, CancellationToken cancellationToken);
}
