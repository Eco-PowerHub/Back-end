using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPowerHub.Migrations
{
    /// <inheritdoc />
    public partial class EditCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "CartItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_PackageId",
                table: "CartItems",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Packages_PackageId",
                table: "CartItems",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Packages_PackageId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_PackageId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "CartItems");
        }
    }
}
