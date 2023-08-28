using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Migrations
{
    public partial class mig_Weblog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weblogs",
                columns: table => new
                {
                    WeblogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    WeblogTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Visit = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeblogBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weblogs", x => x.WeblogId);
                    table.ForeignKey(
                        name: "FK_Weblogs_SubJob_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "SubJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weblogs_CategoryId",
                table: "Weblogs",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weblogs");
        }
    }
}
