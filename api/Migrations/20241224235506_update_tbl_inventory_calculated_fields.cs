using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class update_tbl_inventory_calculated_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalAvailable",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalExpected",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalOnHand",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalOrderd",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAvailable",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "TotalExpected",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "TotalOnHand",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "TotalOrderd",
                table: "Inventories");
        }
    }
}
