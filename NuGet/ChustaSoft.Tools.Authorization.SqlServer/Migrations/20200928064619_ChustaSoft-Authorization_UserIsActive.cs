using Microsoft.EntityFrameworkCore.Migrations;

namespace ChustaSoft.Tools.Authorization.Migrations
{
    public partial class ChustaSoftAuthorization_UserIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Auth",
                table: "Users",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Auth",
                table: "Users");
        }
    }
}
