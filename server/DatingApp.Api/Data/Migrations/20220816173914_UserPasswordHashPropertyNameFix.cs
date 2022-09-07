using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Api.Data.Migrations
{
    public partial class UserPasswordHashPropertyNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaswordHash",
                table: "Users",
                newName: "PasswordHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "PaswordHash");
        }
    }
}
