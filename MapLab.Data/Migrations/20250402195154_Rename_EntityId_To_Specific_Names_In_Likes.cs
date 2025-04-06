using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Rename_EntityId_To_Specific_Names_In_Likes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLikes_Maps_LikeableEntityId",
                table: "MapLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTemplateLikes_MapTemplates_LikeableEntityId",
                table: "MapTemplateLikes");

            migrationBuilder.RenameColumn(
                name: "LikeableEntityId",
                table: "MapTemplateLikes",
                newName: "MapTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_MapTemplateLikes_LikeableEntityId",
                table: "MapTemplateLikes",
                newName: "IX_MapTemplateLikes_MapTemplateId");

            migrationBuilder.RenameColumn(
                name: "LikeableEntityId",
                table: "MapLikes",
                newName: "MapId");

            migrationBuilder.RenameIndex(
                name: "IX_MapLikes_LikeableEntityId",
                table: "MapLikes",
                newName: "IX_MapLikes_MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLikes_Maps_MapId",
                table: "MapLikes",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapTemplateLikes_MapTemplates_MapTemplateId",
                table: "MapTemplateLikes",
                column: "MapTemplateId",
                principalTable: "MapTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLikes_Maps_MapId",
                table: "MapLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTemplateLikes_MapTemplates_MapTemplateId",
                table: "MapTemplateLikes");

            migrationBuilder.RenameColumn(
                name: "MapTemplateId",
                table: "MapTemplateLikes",
                newName: "LikeableEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_MapTemplateLikes_MapTemplateId",
                table: "MapTemplateLikes",
                newName: "IX_MapTemplateLikes_LikeableEntityId");

            migrationBuilder.RenameColumn(
                name: "MapId",
                table: "MapLikes",
                newName: "LikeableEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_MapLikes_MapId",
                table: "MapLikes",
                newName: "IX_MapLikes_LikeableEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLikes_Maps_LikeableEntityId",
                table: "MapLikes",
                column: "LikeableEntityId",
                principalTable: "Maps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapTemplateLikes_MapTemplates_LikeableEntityId",
                table: "MapTemplateLikes",
                column: "LikeableEntityId",
                principalTable: "MapTemplates",
                principalColumn: "Id");
        }
    }
}
