using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addInstructorCourseIdToGeneratedExamTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstructorCourseId",
                table: "Generated_Exams",
                type: "int",
                nullable: true);


            migrationBuilder.AddForeignKey(
                name: "FK_Generated_Exams_Instructor_Courses_InstructorCourseId",
                table: "Generated_Exams",
                column: "InstructorCourseId",
                principalTable: "Instructor_Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Generated_Exams_Instructor_Courses_InstructorCourseId",
                table: "Generated_Exams");


            migrationBuilder.DropColumn(
                name: "InstructorCourseId",
                table: "Generated_Exams");
        }
    }
}
