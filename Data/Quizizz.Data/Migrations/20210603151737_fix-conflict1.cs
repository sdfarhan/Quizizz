using Microsoft.EntityFrameworkCore.Migrations;

namespace Quizizz.Data.Migrations
{
    public partial class fixconflict1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_Quizzes_QuizId",
                table: "Passwords");

            migrationBuilder.DropIndex(
                name: "IX_Passwords_QuizId",
                table: "Passwords");

            migrationBuilder.AlterColumn<string>(
                name: "QuizId",
                table: "Passwords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_PasswordId",
                table: "Quizzes",
                column: "PasswordId",
                unique: true,
                filter: "[PasswordId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Passwords_PasswordId",
                table: "Quizzes",
                column: "PasswordId",
                principalTable: "Passwords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Passwords_PasswordId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_PasswordId",
                table: "Quizzes");

            migrationBuilder.AlterColumn<string>(
                name: "QuizId",
                table: "Passwords",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_QuizId",
                table: "Passwords",
                column: "QuizId",
                unique: true,
                filter: "[QuizId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_Quizzes_QuizId",
                table: "Passwords",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
