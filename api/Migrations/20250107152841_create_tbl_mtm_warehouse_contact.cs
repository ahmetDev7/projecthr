using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class create_tbl_mtm_warehouse_contact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Contacts_ContactId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_ContactId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Warehouses");

            migrationBuilder.CreateTable(
                name: "WarehouseContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseContacts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseContacts_ContactId",
                table: "WarehouseContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseContacts_WarehouseId",
                table: "WarehouseContacts",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseContacts");

            migrationBuilder.AddColumn<Guid>(
                name: "ContactId",
                table: "Warehouses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_ContactId",
                table: "Warehouses",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Contacts_ContactId",
                table: "Warehouses",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
