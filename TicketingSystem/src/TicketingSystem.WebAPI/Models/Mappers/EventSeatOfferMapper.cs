using TicketingSystem.Domain.Models;
using TicketingSystem.WebAPI.Models.Events;

namespace TicketingSystem.WebAPI.Models.Mappers;

public static class EventSeatOfferMapper
{
    public static EventSeatOfferResponse ToResponse(Offer eventSeatOffer)
    {
        return new EventSeatOfferResponse
        {
            OfferId = eventSeatOffer.Id,
            Seat = ToSeatResponse(eventSeatOffer.Seat!),
            Price = eventSeatOffer.Price,
            Status = eventSeatOffer.SeatStatus,
            StatusDescription = eventSeatOffer.SeatStatus.ToString(),
            SeatPriceLevel = eventSeatOffer.SeatPriceLevel?.PriceLevel.ToString() ?? string.Empty
        };
    }

    public static EventSeatResponse ToSeatResponse(Seat seat)
    {
        return new EventSeatResponse
        {
            Id = seat.Id,
            SectionRowId = seat.SectionRowId,
            SectionId = seat.SectionRow?.SectionId ?? Guid.Empty,
            SeatNumber = seat.SeatNumber,
            Type = seat.Type,
            TypeDescription = seat.Type.ToString(),
            SectionRowCode = seat.SectionRow?.Code ?? string.Empty,
            SectionCode = seat.SectionRow?.Section?.Code ?? string.Empty
        };
    }
}
