using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamedTemplateandTemplateIdpropertiesofMapentitytoMapTemplateandMapTemplateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maps_MapTemplates_TemplateId",
                table: "Maps");

            migrationBuilder.RenameColumn(
                name: "TemplateId",
                table: "Maps",
                newName: "MapTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_TemplateId",
                table: "Maps",
                newName: "IX_Maps_MapTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_MapTemplates_MapTemplateId",
                table: "Maps",
                column: "MapTemplateId",
                principalTable: "MapTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maps_MapTemplates_MapTemplateId",
                table: "Maps");

            migrationBuilder.RenameColumn(
                name: "MapTemplateId",
                table: "Maps",
                newName: "TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_MapTemplateId",
                table: "Maps",
                newName: "IX_Maps_TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_MapTemplates_TemplateId",
                table: "Maps",
                column: "TemplateId",
                principalTable: "MapTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
