using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EcoPowerHub.Migrations
{
    /// <inheritdoc />
    public partial class EditUserProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "PackageOrders");

            migrationBuilder.CreateTable(
                name: "UserProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SurfaceArea = table.Column<decimal>(type: "numeric", maxLength: 50, nullable: false),
                    PackagePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Package = table.Column<int>(type: "integer", nullable: false),
                    PackageId = table.Column<int>(type: "integer", nullable: false),
                    ElectricityUsage = table.Column<decimal[]>(type: "numeric[]", nullable: false),
                    TotalYearsGuarantee = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProperties_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProperties_PackageId",
                table: "UserProperties",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "UserProperties");

            migrationBuilder.CreateTable(
                name: "PackageOrders",
                columns: table => new
                {
                    PackageId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ElectricityUsage = table.Column<decimal[]>(type: "numeric[]", nullable: false),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Package = table.Column<int>(type: "integer", nullable: false),
                    PackagePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    SurfaceArea = table.Column<decimal>(type: "numeric", maxLength: 50, nullable: false),
                    TotalYearsGuarantee = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageOrders", x => new { x.PackageId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_PackageOrders_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Companies_CompanyId",
                table: "Products",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
