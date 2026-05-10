using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Application.Interfaces.Services.Commands;
using TicketingSystem.Domain.Interfaces.Services;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Application.Services.Payments;

internal sealed class PaymentCommandService : IPaymentCommandService
{
    private readonly IOrderReadRepository _orderReadRepo;
    private readonly IOfferWriteRepository _offerWriteRepo;
    private readonly IOrderWriteRepository _orderWriteRepo;

    private readonly IOrdersService _ordersService;

    public PaymentCommandService(
        IOrderReadRepository orderReadRepo,
        IOfferWriteRepository offerWriteRepo,
        IOrderWriteRepository orderWriteRepo,
        IOrdersService ordersService)
    {
        _orderReadRepo = orderReadRepo;
        _offerWriteRepo = offerWriteRepo;
        _orderWriteRepo = orderWriteRepo;
        _ordersService = ordersService;
    }

    public async Task<bool> CompletePaymentAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepo.GetByPaymentIdAsync(paymentId, cancellationToken);

        if (!CanModifyOrder(order))
            return false;

        var completionResult = _ordersService.CompleteOrderPayment(order!, paymentId);

        if (!completionResult)
            return false;

        var updateResult = await UpdateOrderStatusAsync(completionResult, order!,
            SeatStatus.Available, cancellationToken);

        return updateResult;
    }

    public async Task<bool> RejectPaymentAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepo.GetByPaymentIdAsync(paymentId, cancellationToken);

        if (!CanModifyOrder(order))
            return false;

        var rejectionResult = _ordersService.RejectOrderPayment(order!, paymentId);

        var updateResult = await UpdateOrderStatusAsync(rejectionResult, order!,
            SeatStatus.Available, cancellationToken);

        return updateResult;
    }

    private async Task<bool> UpdateOrderStatusAsync(bool isOperationCompeted, Order order,
        SeatStatus defaultSeatStatus, CancellationToken cancellationToken)
    {
        if (!isOperationCompeted)
            return false;

        var isOrderUpdated = await _orderWriteRepo.UpdateAsync(order!, cancellationToken);
        var seatStatus = order!.Cart!.Items
            .FirstOrDefault()?.Offer?.SeatStatus ?? defaultSeatStatus;

        var areOffersUpdated = await _offerWriteRepo.UpdateSeatStatusAsync(
            [.. order.Cart!.Items.Select(x => x.OfferId)],
            seatStatus,
            cancellationToken);

        return isOrderUpdated && areOffersUpdated;
    }

    private static bool CanModifyOrder(Order? order)
        => order is not null;
}
