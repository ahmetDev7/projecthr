using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class add_client_order_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BillToClientId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShipToClientId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BillToClientId",
                table: "Orders",
                column: "BillToClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipToClientId",
                table: "Orders",
                column: "ShipToClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_BillToClientId",
                table: "Orders",
                column: "BillToClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ShipToClientId",
                table: "Orders",
                column: "ShipToClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_BillToClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ShipToClientId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BillToClientId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShipToClientId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillToClientId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipToClientId",
                table: "Orders");
        }
    }
}
