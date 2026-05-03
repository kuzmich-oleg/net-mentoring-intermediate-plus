using Microsoft.Extensions.DependencyInjection;
using System.Net;
using TicketingSystem.Application.Interfaces.Repositories;
using TicketingSystem.Common;
using TicketingSystem.DataAccess.SeedData;
using TicketingSystem.WebAPI.Models;
using TicketingSystem.WebAPI.Models.Events;
using TicketingSystem.WebAPI.Models.Orders;

namespace TicketingSystem.WebAPI.IntegrationTests;

public sealed class OrderCreationTests : IClassFixture<WebAppFixture>
{
    private readonly WebAppFixture _fixture;

    public OrderCreationTests(WebAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SearchEventSeats_ValidInput_ValidResponse()
    {
        //get events
        var eventsResponse = await _fixture.Host.Scenario(scenario =>
        {
            scenario.Get.Url($"/events");
            scenario.StatusCodeShouldBe(HttpStatusCode.OK);
        });

        var events = eventsResponse.ReadAsJson<PagedResult<EventResponse>>();

        Assert.NotEmpty(events.Items);

        //get seats for the first event
        var firstEvent = events.Items.First();
        var section = firstEvent.Venue?.Sections.First();

        var eventSeatsResponse = await _fixture.Host.Scenario(scenario =>
        {
            scenario.Get.Url($"/events/{firstEvent.Id}/sections/{section?.Id}/seats");
            scenario.StatusCodeShouldBe(HttpStatusCode.OK);
        });

        var eventSeats = eventSeatsResponse.ReadAsJson<EventSeatOfferResponse[]>();

        Assert.NotEmpty(eventSeats);
    }

    [Fact]
    public async Task GetCart_CartDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var cartId = Guid.NewGuid();

        // Act & Assert
        var response = await _fixture.Host.Scenario(scenario =>
        {
            scenario.Get.Url($"/orders/carts/{cartId}");
            scenario.StatusCodeShouldBe(HttpStatusCode.NotFound);
        });
    }

    //Whole order creation happy path
    [Fact]
    public async Task OrderCompletionHappyPath_ValidInput_ValidResponse()
    {
        //cart creation
        var cartId = Guid.NewGuid();
        var cartRequest = new CartCreationRequest { OfferId = CommonSeedData.Offer1Id };

        await _fixture.Host.Scenario(scenario =>
        {
            scenario.Post.Json(cartRequest).ToUrl($"/orders/carts/{cartId}");
            scenario.StatusCodeShouldBe(HttpStatusCode.Created);
        });

        //add one more cart item
        var addCartItemRequest = new CartCreationRequest { OfferId = CommonSeedData.Offer2Id };

        await _fixture.Host.Scenario(scenario =>
        {
            scenario.Post.Json(addCartItemRequest).ToUrl($"/orders/carts/{cartId}");
            scenario.StatusCodeShouldBe(HttpStatusCode.Created);
        });

        //remove additional cart item
        var deleteItemUrl  =$"/orders/carts/{cartId}/events/{CommonSeedData.Event1Id}/seats/{CommonSeedData.Seat2Id}";

        await _fixture.Host.Scenario(scenario =>
        {
            scenario.Delete.Url(deleteItemUrl);
            scenario.StatusCodeShouldBe(HttpStatusCode.NoContent);
        });

        //book cart
        var bookCartResponse = await _fixture.Host.Scenario(scenario =>
        {
            scenario.Put.Url($"/orders/carts/{cartId}/book");
            scenario.StatusCodeShouldBe(HttpStatusCode.OK);
        });

        //get payment info
        var paymentId = bookCartResponse.ReadAsJson<CartBookingResponse>().PaymentId;

        var paymentInfoResponse = await _fixture.Host.Scenario(scenario =>
        {
            scenario.Get.Url($"/payments/{paymentId}");
            scenario.StatusCodeShouldBe(HttpStatusCode.OK);
        });

        //ensure order is created
        var orderId = paymentInfoResponse.ReadAsJson<PaymentResponse>().OrderId;
        using (var scope = _fixture.Host.Services.CreateScope())
        {
            var orderReadRepo = scope.ServiceProvider.GetRequiredService<IOrderReadRepository>();

            var order = await orderReadRepo.GetByIdAsync(orderId, CancellationToken.None);

            Assert.NotNull(order);
        }

        //complete payment
        await _fixture.Host.Scenario(scenario =>
        {
            scenario.Post.Url($"/payments/{paymentId}/complete");
            scenario.StatusCodeShouldBe(HttpStatusCode.NoContent);
        });
    }
}
