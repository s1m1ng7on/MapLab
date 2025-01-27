using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedfirstversionofMapTemplateEntitywithseedingoftwotemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapTemplates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapTemplates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MapTemplates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "d9d2eefd-2b32-4a0c-a576-50e3848996a4", "Bulgaria" },
                    { "e867e2b7-dda2-4eca-99a1-eea00228a89a", "World" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapTemplates");
        }
    }
}
