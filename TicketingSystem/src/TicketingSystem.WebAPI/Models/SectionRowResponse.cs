namespace TicketingSystem.WebAPI.Models;

public sealed record SectionRowResponse
{
    public Guid Id { get; set; }

    public required string Code { get; set; }

    public SeatResponse[] Seats { get; set; } = [];
}
