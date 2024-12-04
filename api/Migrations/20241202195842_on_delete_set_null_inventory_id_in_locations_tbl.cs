using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class on_delete_set_null_inventory_id_in_locations_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Inventories_InventoryId",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Inventories_InventoryId",
                table: "Locations",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Inventories_InventoryId",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Inventories_InventoryId",
                table: "Locations",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id");
        }
    }
}
