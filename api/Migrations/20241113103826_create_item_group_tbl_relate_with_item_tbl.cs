using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class create_item_group_tbl_relate_with_item_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ItemGroupId",
                table: "Items",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemGroups",
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
                    table.PrimaryKey("PK_ItemGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemGroupId",
                table: "Items",
                column: "ItemGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemGroups_ItemGroupId",
                table: "Items",
                column: "ItemGroupId",
                principalTable: "ItemGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemGroups_ItemGroupId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemGroups");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemGroupId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemGroupId",
                table: "Items");
        }
    }
}
