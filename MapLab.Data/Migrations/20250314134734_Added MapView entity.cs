using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMapViewentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileRecentMapTemplates");

            migrationBuilder.CreateTable(
                name: "MapViews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MapId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapViews_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MapViews_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapViews_MapId",
                table: "MapViews",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_MapViews_ProfileId",
                table: "MapViews",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapViews");

            migrationBuilder.CreateTable(
                name: "ProfileRecentMapTemplates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MapTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileRecentMapTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileRecentMapTemplates_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileRecentMapTemplates_MapTemplates_MapTemplateId",
                        column: x => x.MapTemplateId,
                        principalTable: "MapTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRecentMapTemplates_MapTemplateId",
                table: "ProfileRecentMapTemplates",
                column: "MapTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRecentMapTemplates_ProfileId",
                table: "ProfileRecentMapTemplates",
                column: "ProfileId");
        }
    }
}
