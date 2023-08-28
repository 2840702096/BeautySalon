using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Migrations
{
    public partial class mig_UserCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ValidationCode",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidationCode",
                table: "User");
        }
    }
}
