/*dausing System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteProject.Migrations
{
    public partial class profile2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthTime",
                table: "Profile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IAccept",
                table: "Profile",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthTime",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "IAccept",
                table: "Profile");
        }
    }
}
*/