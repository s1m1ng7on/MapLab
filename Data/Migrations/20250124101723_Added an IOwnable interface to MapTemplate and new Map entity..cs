using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedanIOwnableinterfacetoMapTemplateandnewMapentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MapTemplates",
                keyColumn: "Id",
                keyValue: "482a5f1d-b52a-4a63-952d-58c4beba803d");

            migrationBuilder.DeleteData(
                table: "MapTemplates",
                keyColumn: "Id",
                keyValue: "d2cec3ac-b930-4b00-860d-7ca4ec19f722");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "MapTemplates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Maps_MapTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "MapTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapTemplates_CreatedByUserId",
                table: "MapTemplates",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_CreatedByUserId",
                table: "Maps",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_TemplateId",
                table: "Maps",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapTemplates_AspNetUsers_CreatedByUserId",
                table: "MapTemplates",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapTemplates_AspNetUsers_CreatedByUserId",
                table: "MapTemplates");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropIndex(
                name: "IX_MapTemplates_CreatedByUserId",
                table: "MapTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "MapTemplates");

            migrationBuilder.InsertData(
                table: "MapTemplates",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "IsDeleted", "Name", "UpdatedOn" },
                values: new object[,]
                {
                    { "482a5f1d-b52a-4a63-952d-58c4beba803d", new DateTime(2025, 1, 23, 11, 0, 19, 829, DateTimeKind.Utc).AddTicks(1485), null, false, "World", null },
                    { "d2cec3ac-b930-4b00-860d-7ca4ec19f722", new DateTime(2025, 1, 23, 11, 0, 19, 829, DateTimeKind.Utc).AddTicks(1482), null, false, "Bulgaria", null }
                });
        }
    }
}
