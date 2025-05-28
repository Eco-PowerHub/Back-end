using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPowerHub.Migrations
{
    /// <inheritdoc />
    public partial class EditOnFetch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalYearsGuarantee",
                table: "UserProperties");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserSupport",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSupport_UserId",
                table: "UserSupport",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSupport_AspNetUsers_UserId",
                table: "UserSupport",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSupport_AspNetUsers_UserId",
                table: "UserSupport");

            migrationBuilder.DropIndex(
                name: "IX_UserSupport_UserId",
                table: "UserSupport");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSupport");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<float>(
                name: "TotalYearsGuarantee",
                table: "UserProperties",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
