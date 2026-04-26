using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.SeedData;

internal static class Seats
{
    public static SeatEntity[] DefaultSeats = [
        new SeatEntity
                {
                    Id = CommonSeedData.Seat1Id,
                    SectionRowId = CommonSeedData.Row1Id,
                    SeatNumber = 1,
                    Type = Domain.Models.SeatType.Designated,
                    CreatedAt = CommonSeedData.CreatedAt
                },
            new SeatEntity
                {
                    Id = CommonSeedData.Seat2Id,
                    SectionRowId = CommonSeedData.Row1Id,
                    SeatNumber = 2,
                    Type = Domain.Models.SeatType.Designated,
                    CreatedAt = CommonSeedData.CreatedAt
                },
            new SeatEntity
                {
                    Id = CommonSeedData.Seat3Id,
                    SectionRowId = CommonSeedData.Row2Id,
                    SeatNumber = 1,
                    Type = Domain.Models.SeatType.GeneralAdmission,
                    CreatedAt = CommonSeedData.CreatedAt
                }
        ];
}
