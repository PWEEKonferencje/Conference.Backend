using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affiliations_UserProfiles_UserId",
                table: "Affiliations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Affiliations");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Affiliations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Affiliations_UserProfiles_UserId",
                table: "Affiliations",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Affiliations_UserProfiles_UserId",
                table: "Affiliations");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Affiliations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Affiliations",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Affiliations_UserProfiles_UserId",
                table: "Affiliations",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }
    }
}
