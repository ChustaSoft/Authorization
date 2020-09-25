using Microsoft.EntityFrameworkCore.Migrations;

namespace ChustaSoft.Tools.Authorization.Migrations
{
    public partial class ChustaSoftAuthorization_ConfirmationPropertiesSimplified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                schema: "Auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                schema: "Auth",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                schema: "Auth",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmed",
                schema: "Auth",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                schema: "Auth",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                schema: "Auth",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
