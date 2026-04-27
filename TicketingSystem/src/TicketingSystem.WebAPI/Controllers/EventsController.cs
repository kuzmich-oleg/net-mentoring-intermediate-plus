using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Common;
using TicketingSystem.Common.Extensions;
using TicketingSystem.WebAPI.Models.Events;
using TicketingSystem.WebAPI.Models.Mappers;

namespace TicketingSystem.WebAPI.Controllers;

[Route("events")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventQueryService _eventQueryService;

    public EventsController(IEventQueryService eventQueryService)
    {
        _eventQueryService = eventQueryService;
    }

    [HttpGet]
    [ProducesResponseType<PagedResult<EventResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventsAsync(
        [FromQuery] EventsSearchRequest searchRequest,
        CancellationToken cancellationToken)
    {
        var queryParams = searchRequest.ToQueryParams();

        var queryResult = await _eventQueryService.GetEventsAsync(queryParams, cancellationToken);

        var responseItems = queryResult.Items.MapToArray(EventsMapper.ToResponse);

        var response = new PagedResult<EventResponse>(queryResult.TotalCount, queryResult.OffsetPage, responseItems);

        return Ok(response);
    }

    [HttpGet("{id}/sections/{sectionId}/seats")]
    [ProducesResponseType<EventSeatOfferResponse[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventSeatsAsync(Guid id, Guid sectionId, CancellationToken cancellationToken)
    {
        var offers = await _eventQueryService.GetEventSeatOffersAsync(id, sectionId, cancellationToken);

        if (offers is null)
            return NotFound();

        var responseItems = offers.MapToArray(EventSeatOfferMapper.ToResponse);

        return Ok(responseItems);
    }
}
