using TicketingSystem.Domain.Models;

namespace TicketingSystem.WebAPI.Models.Events;

public class EventSeatResponse
{
    public Guid Id { get; set; }

    public Guid SectionRowId { get; set; }

    public Guid SectionId { get; set; }

    public int SeatNumber { get; set; }

    public SeatType Type { get; set; }

    public required string TypeDescription { get; set; }

    public required string SectionRowCode { get; set; }

    public required string SectionCode { get; set; }
}
