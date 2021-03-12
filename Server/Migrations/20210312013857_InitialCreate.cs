using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Key = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Marketplaces",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(767)", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marketplaces", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User = table.Column<string>(type: "varchar(767)", nullable: false),
                    KeyHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User);
                });

            migrationBuilder.CreateTable(
                name: "Versions",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Marketplace = table.Column<string>(type: "varchar(767)", nullable: false),
                    Branch = table.Column<string>(type: "varchar(767)", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    TextVersion = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Versions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VersionFiles",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    VersionId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    FileKey = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersionFiles_Files_FileKey",
                        column: x => x.FileKey,
                        principalTable: "Files",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionFiles_Versions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "Versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VersionFiles_FileKey",
                table: "VersionFiles",
                column: "FileKey");

            migrationBuilder.CreateIndex(
                name: "IX_VersionFiles_VersionId",
                table: "VersionFiles",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Versions_Branch",
                table: "Versions",
                column: "Branch");

            migrationBuilder.CreateIndex(
                name: "IX_Versions_Marketplace",
                table: "Versions",
                column: "Marketplace");

            migrationBuilder.CreateIndex(
                name: "IX_Versions_Version",
                table: "Versions",
                column: "Version");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marketplaces");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VersionFiles");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Versions");
        }
    }
}
