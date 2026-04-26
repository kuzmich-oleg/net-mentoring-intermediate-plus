using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.SeedData;

internal static class Events
{
    public static EventEntity[] DefaultEvents = [
             new EventEntity
                 {
                     Id = CommonSeedData.Event1Id,
                     Name = "Concert A",
                     Description = "An amazing concert featuring top artists.",
                     EventDate = new DateTimeOffset(2026, 7, 1, 20, 0, 0, TimeSpan.Zero),
                     VenueId = CommonSeedData.VenueId,
                     CreatedAt = CommonSeedData.CreatedAt
                 },
             new EventEntity
                 {
                     Id = CommonSeedData.Event2Id,
                     Name = "Theater B",
                     Description = "A captivating theater performance.",
                     EventDate = new DateTimeOffset(2026, 7, 2, 19, 0, 0, TimeSpan.Zero),
                     VenueId = CommonSeedData.VenueId,
                     CreatedAt = CommonSeedData.CreatedAt
                 },
        ];
}
