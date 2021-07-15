using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemeFolder.EntityFramework.Migrations
{
    public partial class odu9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemeTagNodes_Memes_MemeId",
                table: "MemeTagNodes");

            migrationBuilder.RenameColumn(
                name: "ImageFolderPath",
                table: "Folders",
                newName: "FolderPath");

            migrationBuilder.AlterColumn<Guid>(
                name: "MemeId",
                table: "MemeTagNodes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MemeTagNodes_Memes_MemeId",
                table: "MemeTagNodes",
                column: "MemeId",
                principalTable: "Memes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemeTagNodes_Memes_MemeId",
                table: "MemeTagNodes");

            migrationBuilder.RenameColumn(
                name: "FolderPath",
                table: "Folders",
                newName: "ImageFolderPath");

            migrationBuilder.AlterColumn<Guid>(
                name: "MemeId",
                table: "MemeTagNodes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_MemeTagNodes_Memes_MemeId",
                table: "MemeTagNodes",
                column: "MemeId",
                principalTable: "Memes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
