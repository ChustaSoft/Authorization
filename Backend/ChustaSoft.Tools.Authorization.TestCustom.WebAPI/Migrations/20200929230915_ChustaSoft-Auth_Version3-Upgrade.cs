using Microsoft.EntityFrameworkCore.Migrations;

namespace ChustaSoft.Tools.Authorization.TestCustom.WebAPI.Migrations
{
    public partial class ChustaSoftAuth_Version3Upgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "Auth",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Auth",
                table: "Users",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Auth",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "PhoneNumberIndex",
                schema: "Auth",
                table: "Users",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "PhoneNumberIndex",
                schema: "Auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Auth",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "Auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
