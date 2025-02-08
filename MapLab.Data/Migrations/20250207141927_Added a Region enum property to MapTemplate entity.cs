using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedaRegionenumpropertytoMapTemplateentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Region",
                table: "MapTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProfileRecentMapTemplate",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileRecentMapTemplate");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "MapTemplates");
        }
    }
}
