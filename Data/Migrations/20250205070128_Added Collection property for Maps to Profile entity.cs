using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCollectionpropertyforMapstoProfileentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maps_AspNetUsers_CreatedByUserId",
                table: "Maps");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTemplates_AspNetUsers_CreatedByUserId",
                table: "MapTemplates");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "MapTemplates",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_MapTemplates_CreatedByUserId",
                table: "MapTemplates",
                newName: "IX_MapTemplates_ProfileId");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Maps",
                newName: "ProfileId1");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_CreatedByUserId",
                table: "Maps",
                newName: "IX_Maps_ProfileId1");

            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "Maps",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maps_ProfileId",
                table: "Maps",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_AspNetUsers_ProfileId",
                table: "Maps",
                column: "ProfileId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_AspNetUsers_ProfileId1",
                table: "Maps",
                column: "ProfileId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapTemplates_AspNetUsers_ProfileId",
                table: "MapTemplates",
                column: "ProfileId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maps_AspNetUsers_ProfileId",
                table: "Maps");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_AspNetUsers_ProfileId1",
                table: "Maps");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTemplates_AspNetUsers_ProfileId",
                table: "MapTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Maps_ProfileId",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Maps");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "MapTemplates",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_MapTemplates_ProfileId",
                table: "MapTemplates",
                newName: "IX_MapTemplates_CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "ProfileId1",
                table: "Maps",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_ProfileId1",
                table: "Maps",
                newName: "IX_Maps_CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_AspNetUsers_CreatedByUserId",
                table: "Maps",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapTemplates_AspNetUsers_CreatedByUserId",
                table: "MapTemplates",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
