using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedProfileRecentMapTemplatesmappingentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileRecentMapTemplate");

            migrationBuilder.CreateTable(
                name: "ProfileRecentMapTemplates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MapTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileRecentMapTemplates");

            migrationBuilder.CreateTable(
                name: "ProfileRecentMapTemplate",
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
                    table.PrimaryKey("PK_ProfileRecentMapTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileRecentMapTemplate_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileRecentMapTemplate_MapTemplates_MapTemplateId",
                        column: x => x.MapTemplateId,
                        principalTable: "MapTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRecentMapTemplate_MapTemplateId",
                table: "ProfileRecentMapTemplate",
                column: "MapTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRecentMapTemplate_ProfileId",
                table: "ProfileRecentMapTemplate",
                column: "ProfileId");
        }
    }
}
