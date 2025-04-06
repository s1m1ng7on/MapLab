using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMapLikesandMapTemplateLikesentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapLikes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapLikes_Maps_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MapLikes_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MapTemplateLikes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapTemplateLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapTemplateLikes_MapTemplates_EntityId",
                        column: x => x.EntityId,
                        principalTable: "MapTemplates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MapTemplateLikes_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapLikes_EntityId",
                table: "MapLikes",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MapLikes_ProfileId",
                table: "MapLikes",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MapTemplateLikes_EntityId",
                table: "MapTemplateLikes",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MapTemplateLikes_ProfileId",
                table: "MapTemplateLikes",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapLikes");

            migrationBuilder.DropTable(
                name: "MapTemplateLikes");
        }
    }
}
