using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Application.Services.Events.Models;
using TicketingSystem.Common;
using TicketingSystem.Domain.Models;
using TicketingSystem.WebAPI.Controllers;
using TicketingSystem.WebAPI.Models.Events;

namespace TicketingSystem.WebAPI.UnitTests.Controllers;

public sealed class EventsControllerTests
{
    private readonly Mock<IEventQueryService> _queryServiceMock;

    private readonly EventsController _controller;

    public EventsControllerTests()
    {
        _queryServiceMock = new Mock<IEventQueryService>();
        _controller = new EventsController(_queryServiceMock.Object);
    }

    [Fact]
    public async Task GetEventsAsync_EmptyServiceResponse_ReturnsStatus200()
    {
        //arrange
        const int ZeroTotalCount = 0;
        var fakeOffset = new OffsetPage(1, 10);
        var fakeEmptyResponse = new PagedResult<Event>(ZeroTotalCount, fakeOffset, []);
        var searchRequest = new EventsSearchRequest();

        _queryServiceMock
            .Setup(qs => qs.GetEventsAsync(It.IsAny<EventsQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEmptyResponse);

        //act
        var result = await _controller.GetEventsAsync(searchRequest, CancellationToken.None);

        //assert
        var pagedResult = (result as OkObjectResult)?.Value as PagedResult<EventResponse>;

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(pagedResult);
    }

    [Fact]
    public async Task GetEventsAsync_ValidServiceResponse_ReusesServiceResponseOffset()
    {
        //arrange
        var fakeEvents = new[] { new Event() { Name = "FakeName", Description = "FakeDescription" } };
        var fakeTotalCount = fakeEvents.Length;
        var fakeOffset = new OffsetPage(1, 10);
        var fakeResponse = new PagedResult<Event>(fakeTotalCount, fakeOffset, fakeEvents);
        var searchRequest = new EventsSearchRequest();

        _queryServiceMock
            .Setup(qs => qs.GetEventsAsync(It.IsAny<EventsQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeResponse);

        //act
        var result = await _controller.GetEventsAsync(searchRequest, CancellationToken.None);

        //assert
        var pagedResult = (result as OkObjectResult)?.Value as PagedResult<EventResponse>;

        Assert.NotNull(pagedResult);
        Assert.Equal(fakeOffset, pagedResult.OffsetPage);
    }

    [Fact]
    public async Task GetEventsAsync_ValidServiceResponse_ResponseHasEvents()
    {
        //arrange
        var fakeEvents = new[] { new Event() { Name = "FakeName", Description = "FakeDescription" } };
        var fakeTotalCount = fakeEvents.Length;
        var fakeOffset = new OffsetPage(1, 10);
        var fakeResponse = new PagedResult<Event>(fakeTotalCount, fakeOffset, fakeEvents);
        var searchRequest = new EventsSearchRequest();

        _queryServiceMock
            .Setup(qs => qs.GetEventsAsync(It.IsAny<EventsQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeResponse);

        //act
        var result = await _controller.GetEventsAsync(searchRequest, CancellationToken.None);

        //assert
        var pagedResult = (result as OkObjectResult)?.Value as PagedResult<EventResponse>;

        Assert.NotNull(pagedResult);
        Assert.NotEmpty(pagedResult.Items);
    }

    [Fact]
    public async Task GetEventSeatsAsync_NullServiceResponse_ReturnsStatus404()
    {
        //arrange
        IReadOnlyCollection<Offer>? nullResponse = null;
        var nonExistingId = Guid.NewGuid();

        _queryServiceMock
            .Setup(qs => qs.GetEventSeatOffersAsync(nonExistingId, nonExistingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullResponse);

        //act
        var result = await _controller.GetEventSeatsAsync(nonExistingId, nonExistingId, CancellationToken.None);

        //assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetEventSeatsAsync_ValidServiceResponse_ReturnsStatus200()
    {
        //arrange
        var validServiceResponse = new Offer[] { new() { Seat = new() } };
        var existingId = Guid.NewGuid();

        _queryServiceMock
            .Setup(qs => qs.GetEventSeatOffersAsync(existingId, existingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validServiceResponse);

        //act
        var result = await _controller.GetEventSeatsAsync(existingId, existingId, CancellationToken.None);

        //assert
        var seatOffersArray = (result as OkObjectResult)?.Value as EventSeatOfferResponse[];

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(seatOffersArray);
        Assert.NotEmpty(seatOffersArray);
    }
}
