using TicketingSystem.Domain.Models;
using TicketingSystem.WebAPI.Models.Orders;

namespace TicketingSystem.WebAPI.Models.Mappers;

public static class CartItemSeatMapper
{
    public static CartItemSeatResponse ToResponse(Offer? offer)
    {
        var seat = offer?.Seat
            ?? throw new ArgumentNullException(nameof(offer.Seat));

        return new CartItemSeatResponse
        {
            Id = seat.Id,
            SeatNumber = seat.SeatNumber,
            SectionRowCode = seat.SectionRow?.Code ?? string.Empty,
            SectionCode = seat.SectionRow?.Section?.Code ?? string.Empty
        };
    }
}
