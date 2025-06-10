using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPowerHub.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageTOCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Packages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_CartId",
                table: "Packages",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Carts_CartId",
                table: "Packages",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Carts_CartId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_CartId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Packages");
        }
    }
}
