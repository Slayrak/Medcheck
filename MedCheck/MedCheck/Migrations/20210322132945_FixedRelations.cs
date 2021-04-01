using Microsoft.EntityFrameworkCore.Migrations;

namespace MedCheck.Migrations
{
    public partial class FixedRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_MedWorkers_MedWorkerMedId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MedWorkerMedId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MedWorkerMedId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "MedWorkerUser",
                columns: table => new
                {
                    MedWorkersMedId = table.Column<long>(type: "bigint", nullable: false),
                    UsersUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedWorkerUser", x => new { x.MedWorkersMedId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_MedWorkerUser_MedWorkers_MedWorkersMedId",
                        column: x => x.MedWorkersMedId,
                        principalTable: "MedWorkers",
                        principalColumn: "MedId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedWorkerUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedWorkerUser_UsersUserId",
                table: "MedWorkerUser",
                column: "UsersUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedWorkerUser");

            migrationBuilder.AddColumn<long>(
                name: "MedWorkerMedId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_MedWorkerMedId",
                table: "Users",
                column: "MedWorkerMedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_MedWorkers_MedWorkerMedId",
                table: "Users",
                column: "MedWorkerMedId",
                principalTable: "MedWorkers",
                principalColumn: "MedId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
