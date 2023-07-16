using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    public partial class addedschoolFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolFees_Classes_ClassId",
                table: "SchoolFees");

            migrationBuilder.DropColumn(
                name: "SetDate",
                table: "SchoolFees");

            migrationBuilder.DropColumn(
                name: "TotalFees",
                table: "SchoolFees");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "SchoolFees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                    TotalFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "FK_SchoolFees_Classes_ClassId",
                table: "SchoolFees",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolFees_ClassFees_ClassFeeId",
                table: "SchoolFees",
                column: "ClassFeeId",
                principalTable: "ClassFees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolFees_Classes_ClassId",
                table: "SchoolFees");

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

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "SchoolFees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SetDate",
                table: "SchoolFees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalFees",
                table: "SchoolFees",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolFees_Classes_ClassId",
                table: "SchoolFees",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
