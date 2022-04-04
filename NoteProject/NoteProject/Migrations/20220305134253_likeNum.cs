using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteProject.Migrations
{
    public partial class likeNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoteId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NoteId",
                table: "Users",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Notes_NoteId",
                table: "Users",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Notes_NoteId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_NoteId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Users");
        }
    }
}
