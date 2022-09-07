using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.Api.Data.Migrations
{
    public partial class AddedUserPasswordProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PaswordHash",
                table: "Users",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PaswordSalt",
                table: "Users",
                type: "longblob",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PaswordSalt",
                table: "Users");
        }
    }
}
