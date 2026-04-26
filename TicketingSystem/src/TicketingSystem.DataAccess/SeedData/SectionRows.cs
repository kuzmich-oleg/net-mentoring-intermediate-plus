using TicketingSystem.DataAccess.Entities;

namespace TicketingSystem.DataAccess.SeedData;

internal static class SectionRows
{
    public static SectionRowEntity[] DefaultRows = [
        new SectionRowEntity
            {
                Id = CommonSeedData.Row1Id,
                SectionId = CommonSeedData.Section1Id,
                Code = "Row 1",
                CreatedAt = CommonSeedData.CreatedAt
            },
        new SectionRowEntity
            {
                Id = CommonSeedData.Row2Id,
                SectionId = CommonSeedData.Section1Id,
                Code = "Row 2",
                CreatedAt = CommonSeedData.CreatedAt
            },
        new SectionRowEntity
            {
                Id = CommonSeedData.Row3Id,
                SectionId = CommonSeedData.Section2Id,
                Code = "Row 1",
                CreatedAt = CommonSeedData.CreatedAt
            }
        ];
}
