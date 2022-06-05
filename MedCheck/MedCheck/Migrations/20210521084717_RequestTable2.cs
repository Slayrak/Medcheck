using Microsoft.EntityFrameworkCore.Migrations;

namespace MedCheck.Migrations
{
    public partial class RequestTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientRequests");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderID",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ReceiverId",
                table: "Requests",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_ReceiverId",
                table: "Requests",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_ReceiverId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ReceiverId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SenderID",
                table: "Requests");

            migrationBuilder.CreateTable(
                name: "PatientRequests",
                columns: table => new
                {
                    PatientsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestsRequestId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRequests", x => new { x.PatientsId, x.RequestsRequestId });
                    table.ForeignKey(
                        name: "FK_PatientRequests_AspNetUsers_PatientsId",
                        column: x => x.PatientsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientRequests_Requests_RequestsRequestId",
                        column: x => x.RequestsRequestId,
                        principalTable: "Requests",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientRequests_RequestsRequestId",
                table: "PatientRequests",
                column: "RequestsRequestId");
        }
    }
}
