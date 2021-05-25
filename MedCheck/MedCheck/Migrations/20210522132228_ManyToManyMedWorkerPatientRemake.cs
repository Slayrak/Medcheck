using Microsoft.EntityFrameworkCore.Migrations;

namespace MedCheck.Migrations
{
    public partial class ManyToManyMedWorkerPatientRemake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedWorkerPatient");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedWorkerPatient",
                columns: table => new
                {
                    MedWorkersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedWorkerPatient", x => new { x.MedWorkersId, x.PatientsId });
                    table.ForeignKey(
                        name: "FK_MedWorkerPatient_AspNetUsers_MedWorkersId",
                        column: x => x.MedWorkersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedWorkerPatient_AspNetUsers_PatientsId",
                        column: x => x.PatientsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedWorkerPatient_PatientsId",
                table: "MedWorkerPatient",
                column: "PatientsId");
        }
    }
}
