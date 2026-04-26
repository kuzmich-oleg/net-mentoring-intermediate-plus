using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddedOrderRelatedEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "OrderId",
            schema: "Ticketing",
            table: "Tickets",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<int>(
            name: "SeatStatus",
            schema: "Ticketing",
            table: "Offers",
            type: "int",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.CreateTable(
            name: "Carts",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Carts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Carts_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalSchema: "Ticketing",
                    principalTable: "Customers",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "CartItems",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CartItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_CartItems_Carts_CartId",
                    column: x => x.CartId,
                    principalSchema: "Ticketing",
                    principalTable: "Carts",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_CartItems_Offers_OfferId",
                    column: x => x.OfferId,
                    principalSchema: "Ticketing",
                    principalTable: "Offers",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Orders",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
                table.ForeignKey(
                    name: "FK_Orders_Carts_CartId",
                    column: x => x.CartId,
                    principalSchema: "Ticketing",
                    principalTable: "Carts",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Orders_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalSchema: "Ticketing",
                    principalTable: "Customers",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Payments",
            schema: "Ticketing",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Payments_Orders_OrderId",
                    column: x => x.OrderId,
                    principalSchema: "Ticketing",
                    principalTable: "Orders",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_OrderId",
            schema: "Ticketing",
            table: "Tickets",
            column: "OrderId");

        migrationBuilder.CreateIndex(
            name: "IX_CartItems_CartId_OfferId",
            schema: "Ticketing",
            table: "CartItems",
            columns: new[] { "CartId", "OfferId" },
            unique: true,
            filter: "[IsDeleted] = 0");

        migrationBuilder.CreateIndex(
            name: "IX_CartItems_OfferId",
            schema: "Ticketing",
            table: "CartItems",
            column: "OfferId");

        migrationBuilder.CreateIndex(
            name: "IX_Carts_CustomerId",
            schema: "Ticketing",
            table: "Carts",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_CartId",
            schema: "Ticketing",
            table: "Orders",
            column: "CartId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Orders_CustomerId",
            schema: "Ticketing",
            table: "Orders",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Payments_OrderId",
            schema: "Ticketing",
            table: "Payments",
            column: "OrderId");

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_Orders_OrderId",
            schema: "Ticketing",
            table: "Tickets",
            column: "OrderId",
            principalSchema: "Ticketing",
            principalTable: "Orders",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_Orders_OrderId",
            schema: "Ticketing",
            table: "Tickets");

        migrationBuilder.DropTable(
            name: "CartItems",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Payments",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Orders",
            schema: "Ticketing");

        migrationBuilder.DropTable(
            name: "Carts",
            schema: "Ticketing");

        migrationBuilder.DropIndex(
            name: "IX_Tickets_OrderId",
            schema: "Ticketing",
            table: "Tickets");

        migrationBuilder.DropColumn(
            name: "OrderId",
            schema: "Ticketing",
            table: "Tickets");

        migrationBuilder.DropColumn(
            name: "SeatStatus",
            schema: "Ticketing",
            table: "Offers");
    }
}
