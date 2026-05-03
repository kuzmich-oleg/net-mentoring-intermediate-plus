using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Domain.Models;
using TicketingSystem.WebAPI.Controllers;
using TicketingSystem.WebAPI.Models;

namespace TicketingSystem.WebAPI.UnitTests.Controllers;

public sealed class VenuesControllerTests
{
    private readonly Mock<IVenueQueryService> _queryServiceMock;
    private readonly VenuesController _controller;

    public VenuesControllerTests()
    {
        _queryServiceMock = new Mock<IVenueQueryService>();
        _controller = new VenuesController(_queryServiceMock.Object);
    }

    [Fact]
    public async Task GetVenuesAsync_EmptyServiceResponse_ReturnsStatus200()
    {
        //arrange
        var fakeEmptyResponse = Array.Empty<Venue>();

        _queryServiceMock
            .Setup(qs => qs.GetVenuesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEmptyResponse);

        //act
        var result = await _controller.GetVenuesAsync(CancellationToken.None);

        //assert
        var typedResult = (result as OkObjectResult)?.Value as VenueResponse[];

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(typedResult);
    }

    [Fact]
    public async Task GetVenueSectionsAsync_NullServiceResponse_ReturnsStatus404()
    {
        //arrange
        Venue? nullResponse = null;
        var nonExistingId = Guid.NewGuid();

        _queryServiceMock
            .Setup(qs => qs.GetByIdAsync(nonExistingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullResponse);

        //act
        var result = await _controller.GetVenueSectionsAsync(nonExistingId, CancellationToken.None);

        //assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetVenueSectionsAsync_ValidServiceResponse_ReturnsStatus200()
    {
        //arrange
        var validServiceResponse = new Venue
        {
            Name = "Test Venue",
            Location = "Test Location",
            Sections = [ new Section { Code = "Test Section" } ]
        };
        var existingId = Guid.NewGuid();

        _queryServiceMock
            .Setup(qs => qs.GetByIdAsync(existingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validServiceResponse);

        //act
        var result = await _controller.GetVenueSectionsAsync(existingId, CancellationToken.None);

        //assert
        var sectionsArray = (result as OkObjectResult)?.Value as SectionResponse[];

        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(sectionsArray);
        Assert.NotEmpty(sectionsArray);
    }
}
