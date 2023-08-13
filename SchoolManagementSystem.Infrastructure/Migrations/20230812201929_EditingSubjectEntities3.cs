using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    public partial class EditingSubjectEntities3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Teachers_TeachersId",
                table: "TeacherSubjects");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSubjects_TeachersId",
                table: "TeacherSubjects");

            migrationBuilder.DropColumn(
                name: "TeachersId",
                table: "TeacherSubjects");

            migrationBuilder.RenameColumn(
                name: "TeacherID",
                table: "TeacherSubjects",
                newName: "TeacherId");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "TeacherSubjects",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjects_TeacherId",
                table: "TeacherSubjects",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_Teachers_TeacherId",
                table: "TeacherSubjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Teachers_TeacherId",
                table: "TeacherSubjects");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSubjects_TeacherId",
                table: "TeacherSubjects");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "TeacherSubjects",
                newName: "TeacherID");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherID",
                table: "TeacherSubjects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TeachersId",
                table: "TeacherSubjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjects_TeachersId",
                table: "TeacherSubjects",
                column: "TeachersId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_Teachers_TeachersId",
                table: "TeacherSubjects",
                column: "TeachersId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
