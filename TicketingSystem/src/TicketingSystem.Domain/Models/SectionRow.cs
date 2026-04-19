namespace TicketingSystem.Domain.Models;

public sealed record SectionRow : DomainModelBase
{
    public Guid SectionId { get; set; }

    public required string Code { get; set; }

    public Section? Section { get; set; }
}
