using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddedIsDeletedFilter : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Tickets_CustomerId_OfferId",
            schema: "Ticketing",
            table: "Tickets");

        migrationBuilder.DropIndex(
            name: "IX_Sections_VenueId_Code",
            schema: "Ticketing",
            table: "Sections");

        migrationBuilder.DropIndex(
            name: "IX_SectionRows_SectionId_Code",
            schema: "Ticketing",
            table: "SectionRows");

        migrationBuilder.DropIndex(
            name: "IX_Seats_SectionRowId_SeatNumber",
            schema: "Ticketing",
            table: "Seats");

        migrationBuilder.DropIndex(
            name: "IX_SeatPriceLevels_PriceLevel",
            schema: "Ticketing",
            table: "SeatPriceLevels");

        migrationBuilder.DropIndex(
            name: "IX_Offers_SeatId_EventId_SeatPriceLevelId",
            schema: "Ticketing",
            table: "Offers");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            schema: "Ticketing",
            table: "Events",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_CustomerId_OfferId",
            schema: "Ticketing",
            table: "Tickets",
            columns: new[] { "CustomerId", "OfferId" },
            unique: true,
            filter: "[IsDeleted] = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Sections_VenueId_Code",
            schema: "Ticketing",
            table: "Sections",
            columns: new[] { "VenueId", "Code" },
            unique: true,
            filter: "[IsDeleted] = 0");

        migrationBuilder.CreateIndex(
            name: "IX_SectionRows_SectionId_Code",
            schema: "Ticketing",
            table: "SectionRows",
            columns: new[] { "SectionId", "Code" },
            unique: true,
            filter: "[IsDeleted] = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Seats_SectionRowId_SeatNumber",
            schema: "Ticketing",
            table: "Seats",
            columns: new[] { "SectionRowId", "SeatNumber" },
            unique: true,
            filter: "[IsDeleted] = 0");

        migrationBuilder.CreateIndex(
            name: "IX_SeatPriceLevels_PriceLevel",
            schema: "Ticketing",
            table: "SeatPriceLevels",
            column: "PriceLevel",
            unique: true,
            filter: "[IsDeleted] = 0");

        migrationBuilder.CreateIndex(
            name: "IX_Offers_SeatId_EventId_SeatPriceLevelId",
            schema: "Ticketing",
            table: "Offers",
            columns: new[] { "SeatId", "EventId", "SeatPriceLevelId" },
            unique: true,
            filter: "[IsDeleted] = 0");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Tickets_CustomerId_OfferId",
            schema: "Ticketing",
            table: "Tickets");

        migrationBuilder.DropIndex(
            name: "IX_Sections_VenueId_Code",
            schema: "Ticketing",
            table: "Sections");

        migrationBuilder.DropIndex(
            name: "IX_SectionRows_SectionId_Code",
            schema: "Ticketing",
            table: "SectionRows");

        migrationBuilder.DropIndex(
            name: "IX_Seats_SectionRowId_SeatNumber",
            schema: "Ticketing",
            table: "Seats");

        migrationBuilder.DropIndex(
            name: "IX_SeatPriceLevels_PriceLevel",
            schema: "Ticketing",
            table: "SeatPriceLevels");

        migrationBuilder.DropIndex(
            name: "IX_Offers_SeatId_EventId_SeatPriceLevelId",
            schema: "Ticketing",
            table: "Offers");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            schema: "Ticketing",
            table: "Events",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255);

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_CustomerId_OfferId",
            schema: "Ticketing",
            table: "Tickets",
            columns: new[] { "CustomerId", "OfferId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Sections_VenueId_Code",
            schema: "Ticketing",
            table: "Sections",
            columns: new[] { "VenueId", "Code" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_SectionRows_SectionId_Code",
            schema: "Ticketing",
            table: "SectionRows",
            columns: new[] { "SectionId", "Code" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Seats_SectionRowId_SeatNumber",
            schema: "Ticketing",
            table: "Seats",
            columns: new[] { "SectionRowId", "SeatNumber" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_SeatPriceLevels_PriceLevel",
            schema: "Ticketing",
            table: "SeatPriceLevels",
            column: "PriceLevel",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Offers_SeatId_EventId_SeatPriceLevelId",
            schema: "Ticketing",
            table: "Offers",
            columns: new[] { "SeatId", "EventId", "SeatPriceLevelId" },
            unique: true);
    }
}
