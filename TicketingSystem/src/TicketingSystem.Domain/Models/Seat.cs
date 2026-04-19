namespace TicketingSystem.Domain.Models;

public sealed record Seat : DomainModelBase
{
    public Guid SectionRowId { get; set; }

    public int SeatNumber { get; set; }

    public SeatType Type { get; set; }

    public SectionRow? SectionRow { get; set; }
}
