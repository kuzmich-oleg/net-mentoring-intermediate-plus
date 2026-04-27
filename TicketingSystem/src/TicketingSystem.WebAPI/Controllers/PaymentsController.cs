using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Application.Interfaces.Services.Commands;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Common.Extensions;
using TicketingSystem.WebAPI.Models;
using TicketingSystem.WebAPI.Models.Mappers;

namespace TicketingSystem.WebAPI.Controllers;

[Route("payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentQueryService _paymentQueryService;
    private readonly IOrderCommandService _orderCommandService;

    public PaymentsController(
        IPaymentQueryService paymentQueryService,
        IOrderCommandService orderCommandService)
    {
        _paymentQueryService = paymentQueryService;
        _orderCommandService = orderCommandService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType<PaymentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await GetPaymentResponseAsync(id, cancellationToken);

        return response is null 
            ? NotFound()
            : Ok(response);
    }

    [HttpPost("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompletePaymentAsync(Guid id, CancellationToken cancellationToken)
    {
        var isUpdated = await _orderCommandService.CompletePaymentAsync(id, cancellationToken);

        return isUpdated ? NoContent() : BadRequest();
    }

    [HttpPost("{id}/failed")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RejectPaymentAsync(Guid id, CancellationToken cancellationToken)
    {
        var isUpdated = await _orderCommandService.RejectPaymentAsync(id, cancellationToken);

        return isUpdated ? NoContent() : BadRequest();
    }

    private async Task<PaymentResponse?> GetPaymentResponseAsync(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _paymentQueryService.GetPaymentAsync(id, cancellationToken);
    
        return payment.MapIfNotNull(PaymentMapper.ToResponse);
    }
}
