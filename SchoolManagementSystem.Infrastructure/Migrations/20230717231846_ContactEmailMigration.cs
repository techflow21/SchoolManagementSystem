using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    public partial class ContactEmailMigration : Migration
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

            migrationBuilder.DropColumn(
                name: "SetDate",
                table: "SchoolFees");

            migrationBuilder.DropColumn(
                name: "TotalFees",
                table: "SchoolFees");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "SchoolFees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "SchoolFees");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "SchoolFees",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_ClassId",
                table: "SchoolFees",
                column: "ClassId");

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
