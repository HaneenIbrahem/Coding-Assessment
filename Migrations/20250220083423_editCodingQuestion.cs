using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication4.Migrations
{
    /// <inheritdoc />
    public partial class editCodingQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CodingQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CodingQuestions");
        }
    }
}
