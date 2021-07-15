using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemeFolder.EntityFramework.Migrations
{
    public partial class odu6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Memes",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Memes");
        }
    }
}
