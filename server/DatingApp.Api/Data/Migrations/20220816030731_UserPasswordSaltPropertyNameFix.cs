using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Api.Data.Migrations
{
    public partial class UserPasswordSaltPropertyNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaswordSalt",
                table: "Users",
                newName: "PasswordSalt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "PaswordSalt");
        }
    }
}
