using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Migrations
{
    public partial class mig_timeCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimePeriodText",
                table: "WorkingTime",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimePeriodText",
                table: "WorkingTime");
        }
    }
}
