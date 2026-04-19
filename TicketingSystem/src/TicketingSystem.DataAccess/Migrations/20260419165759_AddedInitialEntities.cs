using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddedInitialEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "Ticketing");

        migrationBuilder.CreateTable(
            name: "SeatPriceLevels",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PriceLevel = table.Column<int>(type: "int", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SeatPriceLevels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<int>(type: "int", nullable: false),
                Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Venues",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Venues", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Customers",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Customers_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "Ticketing",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "EventManagers",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EventManagers", x => x.Id);
                table.ForeignKey(
                    name: "FK_EventManagers_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "Ticketing",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Events",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                EventDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Events", x => x.Id);
                table.ForeignKey(
                    name: "FK_Events_Venues_VenueId",
                    column: x => x.VenueId,
                    principalSchema: "Ticketing",
                    principalTable: "Venues",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Sections",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sections", x => x.Id);
                table.ForeignKey(
                    name: "FK_Sections_Venues_VenueId",
                    column: x => x.VenueId,
                    principalSchema: "Ticketing",
                    principalTable: "Venues",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "SectionRows",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SectionRows", x => x.Id);
                table.ForeignKey(
                    name: "FK_SectionRows_Sections_SectionId",
                    column: x => x.SectionId,
                    principalSchema: "Ticketing",
                    principalTable: "Sections",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Seats",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SectionRowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SeatNumber = table.Column<int>(type: "int", nullable: false),
                Type = table.Column<int>(type: "int", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Seats", x => x.Id);
                table.ForeignKey(
                    name: "FK_Seats_SectionRows_SectionRowId",
                    column: x => x.SectionRowId,
                    principalSchema: "Ticketing",
                    principalTable: "SectionRows",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Offers",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SeatPriceLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Offers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Offers_Events_EventId",
                    column: x => x.EventId,
                    principalSchema: "Ticketing",
                    principalTable: "Events",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Offers_SeatPriceLevels_SeatPriceLevelId",
                    column: x => x.SeatPriceLevelId,
                    principalSchema: "Ticketing",
                    principalTable: "SeatPriceLevels",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Offers_Seats_SeatId",
                    column: x => x.SeatId,
                    principalSchema: "Ticketing",
                    principalTable: "Seats",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Tickets",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tickets", x => x.Id);
                table.ForeignKey(
                    name: "FK_Tickets_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalSchema: "Ticketing",
                    principalTable: "Customers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Tickets_Offers_OfferId",
                    column: x => x.OfferId,
                    principalSchema: "Ticketing",
                    principalTable: "Offers",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Customers_UserId",
            schema: "Ticketing",
            table: "Customers",
            column: "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_EventManagers_UserId",
            schema: "Ticketing",
            table: "EventManagers",
            column: "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Events_VenueId",
            schema: "Ticketing",
            table: "Events",
            column: "VenueId");

        migrationBuilder.CreateIndex(
            name: "IX_Offers_EventId",
            schema: "Ticketing",
            table: "Offers",
            column: "EventId");

        migrationBuilder.CreateIndex(
            name: "IX_Offers_SeatId_EventId_SeatPriceLevelId",
            schema: "Ticketing",
            table: "Offers",
            columns: new[] { "SeatId", "EventId", "SeatPriceLevelId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Offers_SeatPriceLevelId",
            schema: "Ticketing",
            table: "Offers",
            column: "SeatPriceLevelId");

        migrationBuilder.CreateIndex(
            name: "IX_SeatPriceLevels_PriceLevel",
            schema: "Ticketing",
            table: "SeatPriceLevels",
            column: "PriceLevel",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Seats_SectionRowId_SeatNumber",
            schema: "Ticketing",
            table: "Seats",
            columns: new[] { "SectionRowId", "SeatNumber" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_SectionRows_SectionId_Code",
            schema: "Ticketing",
            table: "SectionRows",
            columns: new[] { "SectionId", "Code" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Sections_VenueId_Code",
            schema: "Ticketing",
            table: "Sections",
            columns: new[] { "VenueId", "Code" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_CustomerId_OfferId",
            schema: "Ticketing",
            table: "Tickets",
            columns: new[] { "CustomerId", "OfferId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_OfferId",
            schema: "Ticketing",
            table: "Tickets",
            column: "OfferId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            schema: "Ticketing",
            table: "Users",
            column: "Email",
            unique: true,
            filter: "[IsDeleted] = 0");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EventManagers",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Tickets",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Customers",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Offers",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Events",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "SeatPriceLevels",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Seats",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "SectionRows",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Sections",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Venues",
            schema: "Ticketing");
    }
}
