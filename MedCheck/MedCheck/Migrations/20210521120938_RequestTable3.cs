using Microsoft.EntityFrameworkCore.Migrations;

namespace MedCheck.Migrations
{
    public partial class RequestTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PatientId",
                table: "Requests",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_PatientId",
                table: "Requests",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_PatientId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_PatientId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Requests");
        }
    }
}
