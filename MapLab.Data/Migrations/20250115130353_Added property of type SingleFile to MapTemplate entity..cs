using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedpropertyoftypeSingleFiletoMapTemplateentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MapTemplates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "MapTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MapTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "MapTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MapTemplates",
                keyColumn: "Id",
                keyValue: "d9d2eefd-2b32-4a0c-a576-50e3848996a4",
                columns: new[] { "CreatedOn", "DeletedOn", "IsDeleted", "UpdatedOn" },
                values: new object[] { new DateTime(2025, 1, 15, 13, 3, 52, 711, DateTimeKind.Utc).AddTicks(5740), null, false, null });

            migrationBuilder.UpdateData(
                table: "MapTemplates",
                keyColumn: "Id",
                keyValue: "e867e2b7-dda2-4eca-99a1-eea00228a89a",
                columns: new[] { "CreatedOn", "DeletedOn", "IsDeleted", "UpdatedOn" },
                values: new object[] { new DateTime(2025, 1, 15, 13, 3, 52, 711, DateTimeKind.Utc).AddTicks(5743), null, false, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MapTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "MapTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MapTemplates");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "MapTemplates");
        }
    }
}
