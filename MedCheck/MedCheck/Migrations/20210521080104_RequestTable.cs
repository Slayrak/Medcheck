using Microsoft.EntityFrameworkCore.Migrations;

namespace MedCheck.Migrations
{
    public partial class RequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PrescriptionText",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    RequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestId);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientRequests");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.AlterColumn<string>(
                name: "PrescriptionText",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
