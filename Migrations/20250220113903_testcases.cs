using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication4.Migrations
{
    /// <inheritdoc />
    public partial class testcases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCase_CodingQuestions_CodingQuestionId",
                table: "TestCase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestCase",
                table: "TestCase");

            migrationBuilder.RenameTable(
                name: "TestCase",
                newName: "TestCases");

            migrationBuilder.RenameIndex(
                name: "IX_TestCase_CodingQuestionId",
                table: "TestCases",
                newName: "IX_TestCases_CodingQuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestCases",
                table: "TestCases",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCases_CodingQuestions_CodingQuestionId",
                table: "TestCases",
                column: "CodingQuestionId",
                principalTable: "CodingQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCases_CodingQuestions_CodingQuestionId",
                table: "TestCases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestCases",
                table: "TestCases");

            migrationBuilder.RenameTable(
                name: "TestCases",
                newName: "TestCase");

            migrationBuilder.RenameIndex(
                name: "IX_TestCases_CodingQuestionId",
                table: "TestCase",
                newName: "IX_TestCase_CodingQuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestCase",
                table: "TestCase",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCase_CodingQuestions_CodingQuestionId",
                table: "TestCase",
                column: "CodingQuestionId",
                principalTable: "CodingQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
