using Microsoft.EntityFrameworkCore.Migrations;

namespace MedCheck.Migrations
{
    public partial class FamilyID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserFamilyID",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFamilyID",
                table: "AspNetUsers");
        }
    }
}
