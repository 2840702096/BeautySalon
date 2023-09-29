using Microsoft.EntityFrameworkCore.Migrations;

namespace BeautySalon.Migrations
{
    public partial class mig_HappyClientsCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "HappyClients");

            migrationBuilder.AddColumn<int>(
                name: "Userid",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "User",
                table: "HappyClients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_Userid",
                table: "User",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_HappyClients_User",
                table: "HappyClients",
                column: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_HappyClients_User_User",
                table: "HappyClients",
                column: "User",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_Userid",
                table: "User",
                column: "Userid",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HappyClients_User_User",
                table: "HappyClients");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_Userid",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Userid",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_HappyClients_User",
                table: "HappyClients");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "User");

            migrationBuilder.DropColumn(
                name: "User",
                table: "HappyClients");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "HappyClients",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
