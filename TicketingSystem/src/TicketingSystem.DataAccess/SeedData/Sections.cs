using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.SeedData;

internal static class Sections
{
    public static SectionEntity[] DefaultSections = [
        new SectionEntity
            {
                Id = CommonSeedData.Section1Id,
                VenueId = CommonSeedData.VenueId,
                Code = "Section 1",
                CreatedAt = CommonSeedData.CreatedAt
            },
        new SectionEntity
            {
                Id = CommonSeedData.Section2Id,
                VenueId = CommonSeedData.VenueId,
                Code = "Section 2",
                CreatedAt = CommonSeedData.CreatedAt
            }
        ];
}
