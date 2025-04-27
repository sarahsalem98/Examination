using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examination.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addCountToGeneratedExamTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountMCQ",
                table: "Generated_Exams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountTF",
                table: "Generated_Exams",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountMCQ",
                table: "Generated_Exams");

            migrationBuilder.DropColumn(
                name: "CountTF",
                table: "Generated_Exams");
        }
    }
}
