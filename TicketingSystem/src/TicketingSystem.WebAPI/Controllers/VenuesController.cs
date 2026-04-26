using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Application.Interfaces.Services.Queries;
using TicketingSystem.Common.Extensions;
using TicketingSystem.WebAPI.Models;
using TicketingSystem.WebAPI.Models.Mappers;

namespace TicketingSystem.WebAPI.Controllers;

[Route("venues")]
[ApiController]
public sealed class VenuesController : ControllerBase
{
    private readonly IVenueQueryService _venueQueryService;

    public VenuesController(IVenueQueryService venueQueryService)
    {
        _venueQueryService = venueQueryService;
    }

    [HttpGet]
    [ProducesResponseType<VenueResponse[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVenuesAsync(CancellationToken cancellationToken)
    {
        var venues = await _venueQueryService.GetVenuesAsync(cancellationToken);

        var venuesResponse = venues.MapToArray(VenuesMapper.ToResponse);

        return Ok(venuesResponse);
    }

    [HttpGet("{id}/sections")]
    [ProducesResponseType<SectionResponse[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVenueSectionsAsync(Guid id, CancellationToken cancellationToken)
    {
        var venue = await _venueQueryService.GetByIdAsync(id, cancellationToken);

        if (venue is null)
            return NotFound();

        var response = venue.Sections.MapToArray(SectionsMapper.ToResponse);

        return Ok(response);
    }
}
