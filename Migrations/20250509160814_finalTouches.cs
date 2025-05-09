using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPowerHub.Migrations
{
    /// <inheritdoc />
    public partial class finalTouches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Packages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Packages",
                type: "numeric(12,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
