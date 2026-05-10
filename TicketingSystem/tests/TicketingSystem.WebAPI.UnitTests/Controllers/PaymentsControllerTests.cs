using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketingSystem.Application.Interfaces.Services.Commands;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Domain.Models;
using TicketingSystem.WebAPI.Controllers;
using TicketingSystem.WebAPI.Models;

namespace TicketingSystem.WebAPI.UnitTests.Controllers;

public sealed class PaymentsControllerTests
{
    private readonly Mock<IPaymentQueryService> _queryServiceMock;
    private readonly Mock<IPaymentCommandService> _commandServiceMock;
    private readonly PaymentsController _controller;

    public PaymentsControllerTests()
    {
        _queryServiceMock = new Mock<IPaymentQueryService>();
        _commandServiceMock = new Mock<IPaymentCommandService>();
        _controller = new PaymentsController(_queryServiceMock.Object, _commandServiceMock.Object);
    }

    [Fact]
    public async Task GetPaymentAsync_NullServiceResponse_ReturnsStatus404()
    {
        //arrange
        Payment? nullResponse = null;
        var nonExistingId = Guid.NewGuid();

        _queryServiceMock
            .Setup(qs => qs.GetPaymentAsync(nonExistingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullResponse);

        //act
        var result = await _controller.GetPaymentAsync(nonExistingId, CancellationToken.None);

        //assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetPaymentAsync_ValidServiceResponse_ReturnsStatus200()
    {
        //arrange
        var validServiceResponse = new Payment();
        var existingId = Guid.NewGuid();

        _queryServiceMock
            .Setup(qs => qs.GetPaymentAsync(existingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validServiceResponse);

        //act
        var result = await _controller.GetPaymentAsync(existingId, CancellationToken.None);

        //assert
        var payment = (result as OkObjectResult)?.Value as PaymentResponse;

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(payment);
    }

    [Fact]
    public async Task CompletePaymentAsync_OperationFailed_ReturnsStatus400()
    {
        //arrange
        var fakeId = Guid.NewGuid();

        _commandServiceMock.Setup(cs => cs.CompletePaymentAsync(fakeId, CancellationToken.None))
            .ReturnsAsync(false);

        //act
        var result = await _controller.CompletePaymentAsync(fakeId, CancellationToken.None);

        //assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CompletePaymentAsync_OperationSucceeded_ReturnsStatus204()
    {
        //arrange
        var fakeId = Guid.NewGuid();

        _commandServiceMock.Setup(cs => cs.CompletePaymentAsync(fakeId, CancellationToken.None))
            .ReturnsAsync(true);

        //act
        var result = await _controller.CompletePaymentAsync(fakeId, CancellationToken.None);

        //assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task RejectPaymentAsync_OperationFailed_ReturnsStatus400()
    {
        //arrange
        var fakeId = Guid.NewGuid();

        _commandServiceMock.Setup(cs => cs.RejectPaymentAsync(fakeId, CancellationToken.None))
            .ReturnsAsync(false);

        //act
        var result = await _controller.RejectPaymentAsync(fakeId, CancellationToken.None);

        //assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task RejectPaymentAsync_OperationSucceeded_ReturnsStatus200()
    {
        //arrange
        var fakeId = Guid.NewGuid();

        _commandServiceMock.Setup(cs => cs.RejectPaymentAsync(fakeId, CancellationToken.None))
            .ReturnsAsync(true);

        //act
        var result = await _controller.RejectPaymentAsync(fakeId, CancellationToken.None);

        //assert
        Assert.IsType<NoContentResult>(result);
    }
}
