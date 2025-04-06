using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRequiredattributestosomepropertiestoentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLikes_Maps_MapId",
                table: "MapLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MapLikes_Profiles_ProfileId",
                table: "MapLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTemplateLikes_MapTemplates_MapTemplateId",
                table: "MapTemplateLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTemplateLikes_Profiles_ProfileId",
                table: "MapTemplateLikes");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "MapTemplates",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "MapTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "MapTemplateLikes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MapTemplateId",
                table: "MapTemplateLikes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "MapLikes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MapId",
                table: "MapLikes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MapLikes_Maps_MapId",
                table: "MapLikes",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapLikes_Profiles_ProfileId",
                table: "MapLikes",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MapTemplateLikes_MapTemplates_MapTemplateId",
                table: "MapTemplateLikes",
                column: "MapTemplateId",
                principalTable: "MapTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapTemplateLikes_Profiles_ProfileId",
                table: "MapTemplateLikes",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLikes_Maps_MapId",
                table: "MapLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MapLikes_Profiles_ProfileId",
                table: "MapLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTemplateLikes_MapTemplates_MapTemplateId",
                table: "MapTemplateLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MapTemplateLikes_Profiles_ProfileId",
                table: "MapTemplateLikes");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "MapTemplates",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "MapTemplates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "MapTemplateLikes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "MapTemplateId",
                table: "MapTemplateLikes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "MapLikes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "MapId",
                table: "MapLikes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLikes_Maps_MapId",
                table: "MapLikes",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLikes_Profiles_ProfileId",
                table: "MapLikes",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapTemplateLikes_MapTemplates_MapTemplateId",
                table: "MapTemplateLikes",
                column: "MapTemplateId",
                principalTable: "MapTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapTemplateLikes_Profiles_ProfileId",
                table: "MapTemplateLikes",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }
    }
}
