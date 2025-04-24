using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPowerHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePackagesInheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageOrders_Packages_PackageId",
                table: "PackageOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Companies_CompanyId",
                table: "Packages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.RenameTable(
                name: "Packages",
                newName: "BasePackage");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_CompanyId",
                table: "BasePackage",
                newName: "IX_BasePackage_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasePackage",
                table: "BasePackage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasePackage_Companies_CompanyId",
                table: "BasePackage",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageOrders_BasePackage_PackageId",
                table: "PackageOrders",
                column: "PackageId",
                principalTable: "BasePackage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasePackage_Companies_CompanyId",
                table: "BasePackage");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageOrders_BasePackage_PackageId",
                table: "PackageOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasePackage",
                table: "BasePackage");

            migrationBuilder.RenameTable(
                name: "BasePackage",
                newName: "Packages");

            migrationBuilder.RenameIndex(
                name: "IX_BasePackage_CompanyId",
                table: "Packages",
                newName: "IX_Packages_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageOrders_Packages_PackageId",
                table: "PackageOrders",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Companies_CompanyId",
                table: "Packages",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
