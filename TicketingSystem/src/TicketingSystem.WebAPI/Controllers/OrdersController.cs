using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Application.Interfaces.Services.Commands;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Application.Services.Orders.Models;
using TicketingSystem.Common.Extensions;
using TicketingSystem.WebAPI.Models.Mappers;
using TicketingSystem.WebAPI.Models.Orders;

namespace TicketingSystem.WebAPI.Controllers;

[Route("orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderQueryService _orderQueryService;
    private readonly IOrderCommandService _orderCommandService;

    public OrdersController(
        IOrderQueryService orderQueryService,
        IOrderCommandService orderCommandService)
    {
        _orderQueryService = orderQueryService;
        _orderCommandService = orderCommandService;
    }

    [HttpGet("carts/{cartId}", Name = "GetOrderCart")]
    [ProducesResponseType<CartResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderCartAsync(Guid cartId, CancellationToken cancellationToken)
    {
        var cartResponse = await GetCartResponseAsync(cartId, cancellationToken);

        return cartResponse is null
            ? NotFound()
            : Ok(cartResponse);
    }

    [HttpPost("carts/{cartId}")]
    [ProducesResponseType<CartResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCartAsync(
        [FromRoute] Guid cartId,
        [FromBody] CartCreationRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(cartId);
        var result = await _orderCommandService.UpsertCartAsync(command, cancellationToken);

        if (result is null)
            return BadRequest();

        var cartResponse = await GetCartResponseAsync(cartId, cancellationToken);

        return cartResponse is null
            ? NotFound()
            : CreatedAtRoute("GetOrderCart", new { cartId }, cartResponse);
    }

    [HttpPut("carts/{cartId}/book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BookCartAsync(Guid cartId, CancellationToken cancellationToken)
    {
        var result = await _orderCommandService.CreateOrderAsync(cartId, cancellationToken);

        return result.HasValue 
            ? Ok(new { PaymentId = result.Value })
            : BadRequest();
    }

    [HttpDelete("carts/{cartId}/events/{eventId}/seats/{seatId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCartAsync(Guid cartId, Guid eventId, Guid seatId,
        CancellationToken cancellationToken)
    {
        var deleteCommand = new DeleteCartItemCommand(cartId, eventId, seatId);

        var result = await _orderCommandService.DeleteCartItemAsync(deleteCommand, cancellationToken);

        return !result ? BadRequest() : NoContent();
    }

    private async Task<CartResponse?> GetCartResponseAsync(Guid cartId, CancellationToken cancellationToken)
    {
        var cart = await _orderQueryService.GetCartAsync(cartId, cancellationToken);

        var response = cart.MapIfNotNull(CartMapper.ToResponse);

        return response;
    }
}
