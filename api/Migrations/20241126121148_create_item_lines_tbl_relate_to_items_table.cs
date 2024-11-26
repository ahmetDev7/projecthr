using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class create_item_lines_tbl_relate_to_items_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ItemLineId",
                table: "Items",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLines", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemLineId",
                table: "Items",
                column: "ItemLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemLines_ItemLineId",
                table: "Items",
                column: "ItemLineId",
                principalTable: "ItemLines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemLines_ItemLineId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemLines");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemLineId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemLineId",
                table: "Items");
        }
    }
}
