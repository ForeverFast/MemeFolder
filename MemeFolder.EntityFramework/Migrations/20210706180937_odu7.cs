using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemeFolder.EntityFramework.Migrations
{
    public partial class odu7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemeTags_Folders_FolderId",
                table: "MemeTags");

            migrationBuilder.DropForeignKey(
                name: "FK_MemeTags_Memes_MemeId",
                table: "MemeTags");

            migrationBuilder.DropIndex(
                name: "IX_MemeTags_FolderId",
                table: "MemeTags");

            migrationBuilder.DropIndex(
                name: "IX_MemeTags_MemeId",
                table: "MemeTags");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "MemeTags");

            migrationBuilder.DropColumn(
                name: "MemeId",
                table: "MemeTags");

            migrationBuilder.CreateTable(
                name: "MemeTagNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemeTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemeTagNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemeTagNodes_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemeTagNodes_Memes_MemeId",
                        column: x => x.MemeId,
                        principalTable: "Memes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemeTagNodes_MemeTags_MemeTagId",
                        column: x => x.MemeTagId,
                        principalTable: "MemeTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemeTagNodes_FolderId",
                table: "MemeTagNodes",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_MemeTagNodes_MemeId",
                table: "MemeTagNodes",
                column: "MemeId");

            migrationBuilder.CreateIndex(
                name: "IX_MemeTagNodes_MemeTagId",
                table: "MemeTagNodes",
                column: "MemeTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemeTagNodes");

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                table: "MemeTags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MemeId",
                table: "MemeTags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemeTags_FolderId",
                table: "MemeTags",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_MemeTags_MemeId",
                table: "MemeTags",
                column: "MemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemeTags_Folders_FolderId",
                table: "MemeTags",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemeTags_Memes_MemeId",
                table: "MemeTags",
                column: "MemeId",
                principalTable: "Memes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
