using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConferenceAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine1",
                table: "Conferences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine2",
                table: "Conferences",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Conferences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Conferences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_PlaceName",
                table: "Conferences",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                table: "Conferences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Conferences",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_AddressLine1",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine2",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "Address_PlaceName",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Conferences");
        }
    }
}
