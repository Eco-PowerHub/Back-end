using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPowerHub.Migrations
{
    /// <inheritdoc />
    public partial class EditUserProperty2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProperties_Packages_PackageId",
                table: "UserProperties");

            migrationBuilder.AlterColumn<int>(
                name: "PackageId",
                table: "UserProperties",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProperties_Packages_PackageId",
                table: "UserProperties",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProperties_Packages_PackageId",
                table: "UserProperties");

            migrationBuilder.AlterColumn<int>(
                name: "PackageId",
                table: "UserProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProperties_Packages_PackageId",
                table: "UserProperties",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
