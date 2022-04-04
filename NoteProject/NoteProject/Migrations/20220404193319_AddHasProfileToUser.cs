using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteProject.Migrations
{
    public partial class AddHasProfileToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasProfile",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasProfile",
                table: "Users");
        }
    }
}
