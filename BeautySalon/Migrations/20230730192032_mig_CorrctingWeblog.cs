using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Migrations
{
    public partial class mig_CorrctingWeblog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Weblogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Weblogs");
        }
    }
}
