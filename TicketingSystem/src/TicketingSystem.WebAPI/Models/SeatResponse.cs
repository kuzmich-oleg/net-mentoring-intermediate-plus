using TicketingSystem.Domain.Models;

namespace TicketingSystem.WebAPI.Models;

public sealed record SeatResponse
{
    public Guid Id { get; set; }

    public int SeatNumber { get; set; }

    public SeatType Type { get; set; }
}
