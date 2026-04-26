namespace TicketingSystem.DataAccess.SeedData;

internal static class CommonSeedData
{
    internal static Guid DefaultCustomerId => Guid.Parse("391e388c-57a0-85b8-bf75-019dca57d1f5");

    internal static DateTimeOffset CreatedAt => new(new DateOnly(2026, 1, 1), new TimeOnly(10, 0), TimeSpan.Zero);

    internal static Guid DefaultUserId => Guid.Parse("391e388c-57a0-85b8-bf75-019dca57d1f5");
    internal static string DefaultUserEmail => "test.user@ticketing.domain";

    internal static Guid VenueId => Guid.Parse("162c2be4-42d6-8c0f-badf-019dc8c7bc0e");

    internal static Guid Section1Id => Guid.Parse("e9f97b6c-28cf-8a84-ab83-019dc8d2025f");
    internal static Guid Section2Id => Guid.Parse("08c2bc8d-820f-86c2-aa9f-019dc8d2376a");

    internal static Guid Row1Id => Guid.Parse("d9a7c8e5-9b3e-8f1a-9c83-019dc8d2b0c4");
    internal static Guid Row2Id => Guid.Parse("a1b2c3d4-5678-90ab-cdef-019dc8d2b0c5");
    internal static Guid Row3Id => Guid.Parse("12345678-90ab-cdef-1234-019dc8d2b0c6");

    internal static Guid Seat1Id => Guid.Parse("11111111-2222-3333-4444-019dc8d2b0c7");
    internal static Guid Seat2Id => Guid.Parse("22222222-3333-4444-5555-019dc8d2b0c8");
    internal static Guid Seat3Id => Guid.Parse("33333333-4444-5555-6666-019dc8d2b0c9");

    internal static Guid SeatPriceLevel1Id => Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-019dc8d2b0ca");
    internal static Guid SeatPriceLevel2Id => Guid.Parse("bbbbbbbb-cccc-dddd-eeee-019dc8d2b0cb");
    internal static Guid SeatPriceLevel3Id => Guid.Parse("cccccccc-dddd-eeee-ffff-019dc8d2b0cc");

    internal static Guid Event1Id => Guid.Parse("44444444-5555-6666-7777-019dc8d2b0cd");
    internal static Guid Event2Id => Guid.Parse("55555555-6666-7777-8888-019dc8d2b0ce");

    internal static Guid Offer1Id => Guid.Parse("66666666-7777-8888-9999-019dc8d2b0cf");
    internal static Guid Offer2Id => Guid.Parse("77777777-8888-9999-0000-019dc8d2b0d0");
    internal static Guid Offer3Id => Guid.Parse("88888888-9999-0000-1111-019dc8d2b0d1");
}
