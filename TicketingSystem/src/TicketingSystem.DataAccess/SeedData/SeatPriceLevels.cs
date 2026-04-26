using TicketingSystem.DataAccess.Entities;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.DataAccess.SeedData;

internal static class SeatPriceLevels
{
    public static SeatPriceLevelEntity[] DefaultSeatPriceLevels = [
        new SeatPriceLevelEntity
            {
                Id = CommonSeedData.SeatPriceLevel1Id,
                PriceLevel = SeatPriceLevel.Adult,
                CreatedAt = CommonSeedData.CreatedAt
            },
            new SeatPriceLevelEntity
            {
                Id = CommonSeedData.SeatPriceLevel2Id,
                PriceLevel = SeatPriceLevel.Child,
                CreatedAt = CommonSeedData.CreatedAt
            },
            new SeatPriceLevelEntity
            {
                Id = CommonSeedData.SeatPriceLevel3Id,
                PriceLevel = SeatPriceLevel.VIP,
                CreatedAt = CommonSeedData.CreatedAt
            }
    ];
}
