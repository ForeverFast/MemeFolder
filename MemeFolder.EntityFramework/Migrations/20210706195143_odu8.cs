using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemeFolder.EntityFramework.Migrations
{
    public partial class odu8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemeTagNodes_Folders_FolderId",
                table: "MemeTagNodes");

            migrationBuilder.DropIndex(
                name: "IX_MemeTagNodes_FolderId",
                table: "MemeTagNodes");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "MemeTagNodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                table: "MemeTagNodes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemeTagNodes_FolderId",
                table: "MemeTagNodes",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemeTagNodes_Folders_FolderId",
                table: "MemeTagNodes",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
