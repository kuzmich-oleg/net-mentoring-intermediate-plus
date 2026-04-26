using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketingSystem.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddedSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            schema: "Ticketing",
            table: "SeatPriceLevels",
            columns: new[] { "Id", "CreatedAt", "IsDeleted", "LastModifiedAt", "PriceLevel" },
            values: new object[,]
            {
                { new Guid("aaaaaaaa-bbbb-cccc-dddd-019dc8d2b0ca"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, 1 },
                { new Guid("bbbbbbbb-cccc-dddd-eeee-019dc8d2b0cb"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, 2 },
                { new Guid("cccccccc-dddd-eeee-ffff-019dc8d2b0cc"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, 3 }
            });

        migrationBuilder.InsertData(
            schema: "Ticketing",
            table: "Users",
            columns: new[] { "Id", "CreatedAt", "Email", "IsDeleted", "LastModifiedAt", "Type" },
            values: new object[] { new Guid("391e388c-57a0-85b8-bf75-019dca57d1f5"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "test.user@ticketing.domain", false, null, 1 });

        migrationBuilder.InsertData(
            schema: "Ticketing",
            table: "Venues",
            columns: new[] { "Id", "CreatedAt", "IsDeleted", "LastModifiedAt", "Location", "Name" },
            values: new object[] { new Guid("162c2be4-42d6-8c0f-badf-019dc8c7bc0e"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, "Address 1", "Venue 1" });

        migrationBuilder.InsertData(
            schema: "Ticketing",
            table: "Customers",
            columns: new[] { "Id", "CreatedAt", "FirstName", "IsDeleted", "LastModifiedAt", "LastName", "UserId" },
            values: new object[] { new Guid("391e388c-57a0-85b8-bf75-019dca57d1f5"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Test", false, null, "User", new Guid("391e388c-57a0-85b8-bf75-019dca57d1f5") });

        migrationBuilder.InsertData(
            schema: "Ticketing",
            table: "Events",
            columns: new[] { "Id", "CreatedAt", "Description", "EventDate", "IsDeleted", "LastModifiedAt", "Name", "VenueId" },
            values: new object[,]
            {
                { new Guid("44444444-5555-6666-7777-019dc8d2b0cd"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "An amazing concert featuring top artists.", new DateTimeOffset(new DateTime(2026, 7, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, "Concert A", new Guid("162c2be4-42d6-8c0f-badf-019dc8c7bc0e") },
                { new Guid("55555555-6666-7777-8888-019dc8d2b0ce"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "A captivating theater performance.", new DateTimeOffset(new DateTime(2026, 7, 2, 19, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, "Theater B", new Guid("162c2be4-42d6-8c0f-badf-019dc8c7bc0e") }
            });

        migrationBuilder.InsertData(
            schema: "Ticketing",
            table: "Sections",
            columns: new[] { "Id", "Code", "CreatedAt", "IsDeleted", "LastModifiedAt", "VenueId" },
            values: new object[,]
            {
                { new Guid("08c2bc8d-820f-86c2-aa9f-019dc8d2376a"), "Section 2", new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, new Guid("162c2be4-42d6-8c0f-badf-019dc8c7bc0e") },
                { new Guid("e9f97b6c-28cf-8a84-ab83-019dc8d2025f"), "Section 1", new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, new Guid("162c2be4-42d6-8c0f-badf-019dc8c7bc0e") }
            });

        migrationBuilder.InsertData(
            schema: "Ticketing",
            table: "SectionRows",
            columns: new[] { "Id", "Code", "CreatedAt", "IsDeleted", "LastModifiedAt", "SectionId" },
            values: new object[,]
            {
                { new Guid("12345678-90ab-cdef-1234-019dc8d2b0c6"), "Row 1", new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, new Guid("08c2bc8d-820f-86c2-aa9f-019dc8d2376a") },
                { new Guid("a1b2c3d4-5678-90ab-cdef-019dc8d2b0c5"), "Row 2", new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, new Guid("e9f97b6c-28cf-8a84-ab83-019dc8d2025f") },
                { new Guid("d9a7c8e5-9b3e-8f1a-9c83-019dc8d2b0c4"), "Row 1", new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, new Guid("e9f97b6c-28cf-8a84-ab83-019dc8d2025f") }
            });

        migrationBuilder.InsertData(
            schema: "Ticketing",
            table: "Seats",
            columns: new[] { "Id", "CreatedAt", "IsDeleted", "LastModifiedAt", "SeatNumber", "SectionRowId", "Type" },
            values: new object[,]
            {
                { new Guid("11111111-2222-3333-4444-019dc8d2b0c7"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, 1, new Guid("d9a7c8e5-9b3e-8f1a-9c83-019dc8d2b0c4"), 1 },
                { new Guid("22222222-3333-4444-5555-019dc8d2b0c8"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, 2, new Guid("d9a7c8e5-9b3e-8f1a-9c83-019dc8d2b0c4"), 1 },
                { new Guid("33333333-4444-5555-6666-019dc8d2b0c9"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, 1, new Guid("a1b2c3d4-5678-90ab-cdef-019dc8d2b0c5"), 2 }
            });

        migrationBuilder.InsertData(
            schema: "Ticketing",
            table: "Offers",
            columns: new[] { "Id", "CreatedAt", "EventId", "IsDeleted", "LastModifiedAt", "Price", "SeatId", "SeatPriceLevelId" },
            values: new object[,]
            {
                { new Guid("66666666-7777-8888-9999-019dc8d2b0cf"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("44444444-5555-6666-7777-019dc8d2b0cd"), false, null, 100m, new Guid("11111111-2222-3333-4444-019dc8d2b0c7"), new Guid("aaaaaaaa-bbbb-cccc-dddd-019dc8d2b0ca") },
                { new Guid("77777777-8888-9999-0000-019dc8d2b0d0"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("44444444-5555-6666-7777-019dc8d2b0cd"), false, null, 50m, new Guid("22222222-3333-4444-5555-019dc8d2b0c8"), new Guid("bbbbbbbb-cccc-dddd-eeee-019dc8d2b0cb") },
                { new Guid("88888888-9999-0000-1111-019dc8d2b0d1"), new DateTimeOffset(new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("44444444-5555-6666-7777-019dc8d2b0cd"), false, null, 200m, new Guid("33333333-4444-5555-6666-019dc8d2b0c9"), new Guid("cccccccc-dddd-eeee-ffff-019dc8d2b0cc") }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Customers",
            keyColumn: "Id",
            keyValue: new Guid("391e388c-57a0-85b8-bf75-019dca57d1f5"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Events",
            keyColumn: "Id",
            keyValue: new Guid("55555555-6666-7777-8888-019dc8d2b0ce"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Offers",
            keyColumn: "Id",
            keyValue: new Guid("66666666-7777-8888-9999-019dc8d2b0cf"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Offers",
            keyColumn: "Id",
            keyValue: new Guid("77777777-8888-9999-0000-019dc8d2b0d0"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Offers",
            keyColumn: "Id",
            keyValue: new Guid("88888888-9999-0000-1111-019dc8d2b0d1"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "SectionRows",
            keyColumn: "Id",
            keyValue: new Guid("12345678-90ab-cdef-1234-019dc8d2b0c6"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Events",
            keyColumn: "Id",
            keyValue: new Guid("44444444-5555-6666-7777-019dc8d2b0cd"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "SeatPriceLevels",
            keyColumn: "Id",
            keyValue: new Guid("aaaaaaaa-bbbb-cccc-dddd-019dc8d2b0ca"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "SeatPriceLevels",
            keyColumn: "Id",
            keyValue: new Guid("bbbbbbbb-cccc-dddd-eeee-019dc8d2b0cb"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "SeatPriceLevels",
            keyColumn: "Id",
            keyValue: new Guid("cccccccc-dddd-eeee-ffff-019dc8d2b0cc"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Seats",
            keyColumn: "Id",
            keyValue: new Guid("11111111-2222-3333-4444-019dc8d2b0c7"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Seats",
            keyColumn: "Id",
            keyValue: new Guid("22222222-3333-4444-5555-019dc8d2b0c8"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Seats",
            keyColumn: "Id",
            keyValue: new Guid("33333333-4444-5555-6666-019dc8d2b0c9"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Sections",
            keyColumn: "Id",
            keyValue: new Guid("08c2bc8d-820f-86c2-aa9f-019dc8d2376a"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("391e388c-57a0-85b8-bf75-019dca57d1f5"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "SectionRows",
            keyColumn: "Id",
            keyValue: new Guid("a1b2c3d4-5678-90ab-cdef-019dc8d2b0c5"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "SectionRows",
            keyColumn: "Id",
            keyValue: new Guid("d9a7c8e5-9b3e-8f1a-9c83-019dc8d2b0c4"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Sections",
            keyColumn: "Id",
            keyValue: new Guid("e9f97b6c-28cf-8a84-ab83-019dc8d2025f"));

        migrationBuilder.DeleteData(
            schema: "Ticketing",
            table: "Venues",
            keyColumn: "Id",
            keyValue: new Guid("162c2be4-42d6-8c0f-badf-019dc8c7bc0e"));
    }
}
