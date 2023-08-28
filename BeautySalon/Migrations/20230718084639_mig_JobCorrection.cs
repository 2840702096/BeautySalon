using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Migrations
{
    public partial class mig_JobCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Job_JobId",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_Job_ParentId",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Job_ParentId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "ParentName",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "ReservationCost",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Job");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "Admin",
                newName: "SubJobId");

            migrationBuilder.RenameIndex(
                name: "IX_Admin_JobId",
                table: "Admin",
                newName: "IX_Admin_SubJobId");

            migrationBuilder.CreateTable(
                name: "SubJob",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ParentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: true),
                    ReservationCost = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubJob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubJob_Job_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubJob_ParentId",
                table: "SubJob",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_SubJob_SubJobId",
                table: "Admin",
                column: "SubJobId",
                principalTable: "SubJob",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_SubJob_SubJobId",
                table: "Admin");

            migrationBuilder.DropTable(
                name: "SubJob");

            migrationBuilder.RenameColumn(
                name: "SubJobId",
                table: "Admin",
                newName: "JobId");

            migrationBuilder.RenameIndex(
                name: "IX_Admin_SubJobId",
                table: "Admin",
                newName: "IX_Admin_JobId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Job",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Job",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentName",
                table: "Job",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Job",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationCost",
                table: "Job",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Job",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Job_ParentId",
                table: "Job",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Job_JobId",
                table: "Admin",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Job_ParentId",
                table: "Job",
                column: "ParentId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
