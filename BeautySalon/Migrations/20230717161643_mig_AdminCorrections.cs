using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Migrations
{
    public partial class mig_AdminCorrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JopName",
                table: "Admin",
                newName: "JobName");

            migrationBuilder.AlterColumn<int>(
                name: "Percent",
                table: "Admin",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobName",
                table: "Admin",
                newName: "JopName");

            migrationBuilder.AlterColumn<int>(
                name: "Percent",
                table: "Admin",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
