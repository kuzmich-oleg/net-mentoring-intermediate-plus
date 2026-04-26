namespace TicketingSystem.Application.Services.Orders.Models;

public sealed record CreateCartCommand
{
    //client generates cart id
    public Guid CartId { get; set; }

    public Guid OfferId { get; init; }
}
