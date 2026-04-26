namespace TicketingSystem.WebAPI.Models;

public sealed record SectionResponse
{
    public Guid Id { get; set; }

    public required string Code { get; set; }

    public SectionRowResponse[] Rows { get; set; } = [];
}
