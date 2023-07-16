using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    public partial class UpdatedSchoolFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolFees_Classes_ClassId",
                table: "SchoolFees");

            migrationBuilder.DropIndex(
                name: "IX_SchoolFees_ClassId",
                table: "SchoolFees");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "SchoolFees");

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "SchoolFees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "SchoolFees");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "SchoolFees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_ClassId",
                table: "SchoolFees",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolFees_Classes_ClassId",
                table: "SchoolFees",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");
        }
    }
}
