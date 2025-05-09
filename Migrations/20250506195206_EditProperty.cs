using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPowerHub.Migrations
{
    /// <inheritdoc />
    public partial class EditProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Package",
                table: "UserProperties",
                newName: "PackageType");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalYearsGuarantee",
                table: "UserProperties",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "UserProperties",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerYear",
                table: "UserProperties",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ROIYears",
                table: "UserProperties",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SavingCost",
                table: "UserProperties",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerYear",
                table: "UserProperties");

            migrationBuilder.DropColumn(
                name: "ROIYears",
                table: "UserProperties");

            migrationBuilder.DropColumn(
                name: "SavingCost",
                table: "UserProperties");

            migrationBuilder.RenameColumn(
                name: "PackageType",
                table: "UserProperties",
                newName: "Package");

            migrationBuilder.AlterColumn<float>(
                name: "TotalYearsGuarantee",
                table: "UserProperties",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "UserProperties",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
