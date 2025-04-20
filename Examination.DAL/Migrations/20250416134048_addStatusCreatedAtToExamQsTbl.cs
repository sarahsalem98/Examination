using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addStatusCreatedAtToExamQsTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Exam_Qs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Exam_Qs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Exam_Qs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Exam_Qs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Exam_Qs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Exam_Qs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Exam_Qs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Exam_Qs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Exam_Qs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Exam_Qs");
        }
    }
}
