using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.SeedData;

internal static class Offers
{
    public static OfferEntity[] DefaultOffers = [
        new OfferEntity
        {
            Id = CommonSeedData.Offer1Id,
            EventId = CommonSeedData.Event1Id,
            SeatId = CommonSeedData.Seat1Id,
            SeatPriceLevelId = CommonSeedData.SeatPriceLevel1Id,
            Price = 100m,
            CreatedAt = CommonSeedData.CreatedAt
        },
        new OfferEntity
        {
            Id = CommonSeedData.Offer2Id,
            EventId = CommonSeedData.Event1Id,
            SeatId = CommonSeedData.Seat2Id,
            SeatPriceLevelId = CommonSeedData.SeatPriceLevel2Id,
            Price = 50m,
            CreatedAt = CommonSeedData.CreatedAt
        },
        new OfferEntity
        {
            Id = CommonSeedData.Offer3Id,
            EventId = CommonSeedData.Event1Id,
            SeatId = CommonSeedData.Seat3Id,
            SeatPriceLevelId = CommonSeedData.SeatPriceLevel3Id,
            Price = 200m,
            CreatedAt = CommonSeedData.CreatedAt
        }
    ];
}
