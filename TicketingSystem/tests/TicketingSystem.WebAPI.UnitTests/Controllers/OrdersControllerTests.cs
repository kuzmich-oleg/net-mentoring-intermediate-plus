using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketingSystem.Application.Interfaces.Services.Commands;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Application.Services.Orders.Models;
using TicketingSystem.Domain.Models;
using TicketingSystem.WebAPI.Controllers;
using TicketingSystem.WebAPI.Models.Orders;

namespace TicketingSystem.WebAPI.UnitTests.Controllers;

public sealed class OrdersControllerTests
{
    private readonly Mock<IOrderQueryService> _queryServiceMock;
    private readonly Mock<IOrderCommandService> _commandServiceMock;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _queryServiceMock = new Mock<IOrderQueryService>();
        _commandServiceMock = new Mock<IOrderCommandService>();
        _controller = new OrdersController(_queryServiceMock.Object, _commandServiceMock.Object);
    }

    [Fact]
    public async Task GetOrderCartAsync_NullServiceResponse_ReturnsStatus404()
    {
        //arrange
        Cart? nullResponse = null;
        var nonExistingId = Guid.NewGuid();

        _queryServiceMock
            .Setup(qs => qs.GetCartAsync(nonExistingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullResponse);

        //act
        var result = await _controller.GetOrderCartAsync(nonExistingId, CancellationToken.None);

        //assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetOrderCartAsync_ValidServiceResponse_ReturnsStatus200()
    {
        //arrange
        var validServiceResponse = new Cart();
        var existingId = Guid.NewGuid();

        _queryServiceMock
            .Setup(qs => qs.GetCartAsync(existingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validServiceResponse);

        //act
        var result = await _controller.GetOrderCartAsync(existingId, CancellationToken.None);

        //assert
        var cart = (result as OkObjectResult)?.Value as CartResponse;

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(cart);
    }

    [Fact]
    public async Task CreateCartAsync_UpsertFailed_ReturnsStatus400()
    {
        //arrange
        Guid? nullResponse = null;
        var fakeCartId = Guid.NewGuid();
        var createCartRequest = new CartCreationRequest { OfferId = Guid.NewGuid() };
        var command = new CreateCartCommand { CartId = fakeCartId, OfferId = createCartRequest.OfferId };

        _commandServiceMock
            .Setup(qs => qs.UpsertCartAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullResponse);

        //act
        var result = await _controller.CreateCartAsync(fakeCartId, createCartRequest, CancellationToken.None);

        //assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateCartAsync_UpsertCompletedButGetFailed_ReturnsStatus404()
    {
        //arrange
        Cart? nullCartResponse = null;
        var cartId = Guid.NewGuid();
        var createCartRequest = new CartCreationRequest { OfferId = Guid.NewGuid() };
        var command = new CreateCartCommand { CartId = cartId, OfferId = createCartRequest.OfferId };

        _commandServiceMock
            .Setup(qs => qs.UpsertCartAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartId);

        _queryServiceMock
            .Setup(qs => qs.GetCartAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullCartResponse);

        //act
        var result = await _controller.CreateCartAsync(cartId, createCartRequest, CancellationToken.None);

        //assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateCartAsync_UpsertCompleted_ReturnsStatus201()
    {
        //arrange
        var cartResponse = new Cart();
        var cartId = Guid.NewGuid();
        var createCartRequest = new CartCreationRequest { OfferId = Guid.NewGuid() };
        var command = new CreateCartCommand { CartId = cartId, OfferId = createCartRequest.OfferId };

        _commandServiceMock
            .Setup(qs => qs.UpsertCartAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartId);

        _queryServiceMock
            .Setup(qs => qs.GetCartAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartResponse);

        //act
        var result = await _controller.CreateCartAsync(cartId, createCartRequest, CancellationToken.None);

        //assert
        var cart = (result as CreatedAtRouteResult)?.Value as CartResponse;

        Assert.IsType<CreatedAtRouteResult>(result);
        Assert.NotNull(cart);
    }

    [Fact]
    public async Task BookCartAsync_OrderCreationFailed_ReturnsStatus400()
    {
        //arrange
        Guid? nullResponse = null;
        var fakeCartId = Guid.NewGuid();

        _commandServiceMock
            .Setup(qs => qs.CreateOrderAsync(fakeCartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullResponse);

        //act
        var result = await _controller.BookCartAsync(fakeCartId, CancellationToken.None);

        //assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task BookCartAsync_OrderCreationSucceeded_ReturnsStatus200()
    {
        //arrange
        Guid? paymentIdResponse = Guid.NewGuid();
        var fakeCartId = Guid.NewGuid();

        _commandServiceMock
            .Setup(qs => qs.CreateOrderAsync(fakeCartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentIdResponse);

        //act
        var result = await _controller.BookCartAsync(fakeCartId, CancellationToken.None);

        //assert
        var resultValue = (result as OkObjectResult)?.Value;

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(resultValue);
    }

    [Fact]
    public async Task DeleteCartAsync_OperationFailed_ReturnsStatus400()
    {
        //arrange
        var fakeCartId = Guid.NewGuid();
        var fakeEventId = Guid.NewGuid();
        var fakeSeatId = Guid.NewGuid();
        var deleteItemCommand = new DeleteCartItemCommand(fakeCartId, fakeEventId, fakeSeatId);

        _commandServiceMock
            .Setup(qs => qs.DeleteCartItemAsync(deleteItemCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //act
        var result = await _controller.DeleteCartAsync(fakeCartId, fakeEventId, fakeSeatId, CancellationToken.None);

        //assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task DeleteCartAsync_OperationSucceeded_ReturnsStatus204()
    {
        //arrange
        var fakeCartId = Guid.NewGuid();
        var fakeEventId = Guid.NewGuid();
        var fakeSeatId = Guid.NewGuid();
        var deleteItemCommand = new DeleteCartItemCommand(fakeCartId, fakeEventId, fakeSeatId);

        _commandServiceMock
            .Setup(qs => qs.DeleteCartItemAsync(deleteItemCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        //act
        var result = await _controller.DeleteCartAsync(fakeCartId, fakeEventId, fakeSeatId, CancellationToken.None);

        //assert
        Assert.IsType<NoContentResult>(result);
    }
}
