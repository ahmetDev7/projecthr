using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class create_shipments_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShipmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShipmentType = table.Column<string>(type: "text", nullable: false),
                    ShipmentStatus = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CarrierCode = table.Column<string>(type: "text", nullable: false),
                    CarrierDescription = table.Column<string>(type: "text", nullable: true),
                    ServiceCode = table.Column<string>(type: "text", nullable: false),
                    PaymentType = table.Column<string>(type: "text", nullable: false),
                    TransferMode = table.Column<string>(type: "text", nullable: false),
                    TotalPackageAmount = table.Column<string>(type: "text", nullable: true),
                    TotalPackageCount = table.Column<int>(type: "integer", nullable: true),
                    TotalPackageWeight = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shipments");
        }
    }
}
