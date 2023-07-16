using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    public partial class SchoolFeeAdjusted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolFees_ClassFees_ClassFeeId",
                table: "SchoolFees");

            migrationBuilder.DropTable(
                name: "ClassFees");

            migrationBuilder.DropIndex(
                name: "IX_SchoolFees_ClassFeeId",
                table: "SchoolFees");

            migrationBuilder.DropColumn(
                name: "ClassFeeId",
                table: "SchoolFees");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "SchoolFees",
                newName: "Class");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Class",
                table: "SchoolFees",
                newName: "ClassName");

            migrationBuilder.AddColumn<int>(
                name: "ClassFeeId",
                table: "SchoolFees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClassFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassFees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_ClassFeeId",
                table: "SchoolFees",
                column: "ClassFeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolFees_ClassFees_ClassFeeId",
                table: "SchoolFees",
                column: "ClassFeeId",
                principalTable: "ClassFees",
                principalColumn: "Id");
        }
    }
}
