using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Application.Interfaces.Services.Commands;
using TicketingSystem.Application.Interfaces.Services.Queries;
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
        var cart = await _orderQueryService.GetCartAsync(cartId, cancellationToken);

        if (cart is null)
            return NotFound();

        var cartResponse = CartMapper.ToResponse(cart);

        return Ok(cartResponse);
    }

    [HttpPost("carts/{cartId}")]
    [ProducesResponseType<CartResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCartAsync(
        [FromRoute] Guid cartId,
        [FromBody] CartCreationRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(cartId);
        var result = await _orderCommandService.CreateCartAsync(command, cancellationToken);

        if (result is null)
            return BadRequest();

        var cart = await _orderQueryService.GetCartAsync(result.Value, cancellationToken);

        if (cart is null)
            return BadRequest();

        var cartResponse = CartMapper.ToResponse(cart);
        return CreatedAtRoute("GetOrderCart", new { cartId }, cartResponse);
    }
}
