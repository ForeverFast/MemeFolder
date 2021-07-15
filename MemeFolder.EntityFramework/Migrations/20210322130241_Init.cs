using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemeFolder.EntityFramework.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageFolderPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Memes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memes_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemeTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemeTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemeTags_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemeTags_Memes_MemeId",
                        column: x => x.MemeId,
                        principalTable: "Memes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Memes_FolderId",
                table: "Memes",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_MemeTags_FolderId",
                table: "MemeTags",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_MemeTags_MemeId",
                table: "MemeTags",
                column: "MemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemeTags");

            migrationBuilder.DropTable(
                name: "Memes");

            migrationBuilder.DropTable(
                name: "Folders");
        }
    }
}
