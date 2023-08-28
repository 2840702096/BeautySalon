using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Migrations
{
    public partial class mig_WorkingTimeCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DayDate",
                table: "WorkingTime",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayDate",
                table: "WorkingTime");
        }
    }
}
