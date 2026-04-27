using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Services.Payments;

internal sealed class PaymentQueryService : IPaymentQueryService
{
    private readonly IPaymentReadRepository _paymentReadRepo;

    public PaymentQueryService(IPaymentReadRepository paymentReadRepo)
    {
        _paymentReadRepo = paymentReadRepo;
    }

    public async Task<Payment?> GetPaymentAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        var payment = await _paymentReadRepo.GetByIdAsync(paymentId, cancellationToken);
        
        return payment;
    }
}
