using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addAddtionalDataToInstructorCourseTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectivePassedStudentCount",
                table: "Instructor_Courses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Instructor_Courses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalPassedStudentCount",
                table: "Instructor_Courses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastGeneratedExamType",
                table: "Instructor_Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Instructor_Courses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalStudents",
                table: "Instructor_Courses",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectivePassedStudentCount",
                table: "Instructor_Courses");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Instructor_Courses");

            migrationBuilder.DropColumn(
                name: "FinalPassedStudentCount",
                table: "Instructor_Courses");

            migrationBuilder.DropColumn(
                name: "LastGeneratedExamType",
                table: "Instructor_Courses");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Instructor_Courses");

            migrationBuilder.DropColumn(
                name: "TotalStudents",
                table: "Instructor_Courses");
        }
    }
}
