namespace Quizizz.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Updated_Some_entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCorrectAnswer",
                table: "Answers",
                newName: "IsRightAnswer");

            migrationBuilder.AddColumn<string>(
                name: "QuizName",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuizName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "IsRightAnswer",
                table: "Answers",
                newName: "IsCorrectAnswer");
        }
    }
}
