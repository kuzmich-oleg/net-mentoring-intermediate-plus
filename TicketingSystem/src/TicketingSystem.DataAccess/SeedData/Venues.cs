using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.SeedData;

internal static class Venues
{
    public static VenueEntity[] DefaultVenues = [
        new VenueEntity
        {
            Id = CommonSeedData.VenueId,
            Name = "Venue 1",
            Location = "Address 1",
            CreatedAt = CommonSeedData.CreatedAt
        },
    ];
}
