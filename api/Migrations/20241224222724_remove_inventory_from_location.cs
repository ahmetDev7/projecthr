using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class remove_inventory_from_location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Inventories_InventoryId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_InventoryId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Locations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InventoryId",
                table: "Locations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_InventoryId",
                table: "Locations",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Inventories_InventoryId",
                table: "Locations",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
