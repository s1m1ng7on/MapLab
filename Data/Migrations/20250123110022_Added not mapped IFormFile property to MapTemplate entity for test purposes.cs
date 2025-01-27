using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddednotmappedIFormFilepropertytoMapTemplateentityfortestpurposes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MapTemplates",
                keyColumn: "Id",
                keyValue: "d9d2eefd-2b32-4a0c-a576-50e3848996a4");

            migrationBuilder.DeleteData(
                table: "MapTemplates",
                keyColumn: "Id",
                keyValue: "e867e2b7-dda2-4eca-99a1-eea00228a89a");

            migrationBuilder.InsertData(
                table: "MapTemplates",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "IsDeleted", "Name", "UpdatedOn" },
                values: new object[,]
                {
                    { "482a5f1d-b52a-4a63-952d-58c4beba803d", new DateTime(2025, 1, 23, 11, 0, 19, 829, DateTimeKind.Utc).AddTicks(1485), null, false, "World", null },
                    { "d2cec3ac-b930-4b00-860d-7ca4ec19f722", new DateTime(2025, 1, 23, 11, 0, 19, 829, DateTimeKind.Utc).AddTicks(1482), null, false, "Bulgaria", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MapTemplates",
                keyColumn: "Id",
                keyValue: "482a5f1d-b52a-4a63-952d-58c4beba803d");

            migrationBuilder.DeleteData(
                table: "MapTemplates",
                keyColumn: "Id",
                keyValue: "d2cec3ac-b930-4b00-860d-7ca4ec19f722");

            migrationBuilder.InsertData(
                table: "MapTemplates",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "IsDeleted", "Name", "UpdatedOn" },
                values: new object[,]
                {
                    { "d9d2eefd-2b32-4a0c-a576-50e3848996a4", new DateTime(2025, 1, 15, 13, 3, 52, 711, DateTimeKind.Utc).AddTicks(5740), null, false, "Bulgaria", null },
                    { "e867e2b7-dda2-4eca-99a1-eea00228a89a", new DateTime(2025, 1, 15, 13, 3, 52, 711, DateTimeKind.Utc).AddTicks(5743), null, false, "World", null }
                });
        }
    }
}
