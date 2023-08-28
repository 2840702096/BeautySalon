using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Migrations
{
    public partial class mig_InitialDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Job",
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
                    table.PrimaryKey("PK_Job", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Job_Job_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WorkingDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    JopName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AdminRole = table.Column<int>(type: "int", nullable: false),
                    Percent = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admin_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkingTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    DayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    AdminName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ReservationCost = table.Column<int>(type: "int", nullable: false),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingTime_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkingTime_WorkingDays_DayId",
                        column: x => x.DayId,
                        principalTable: "WorkingDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkingTimeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ReservationCost = table.Column<int>(type: "int", nullable: false),
                    FinalPayment = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    JopName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    AdminName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_WorkingTime_WorkingTimeId",
                        column: x => x.WorkingTimeId,
                        principalTable: "WorkingTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admin_JobId",
                table: "Admin",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_ParentId",
                table: "Job",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AdminId",
                table: "Reservations",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_WorkingTimeId",
                table: "Reservations",
                column: "WorkingTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTime_AdminId",
                table: "WorkingTime",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTime_DayId",
                table: "WorkingTime",
                column: "DayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "WorkingTime");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "WorkingDays");

            migrationBuilder.DropTable(
                name: "Job");
        }
    }
}
