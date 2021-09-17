using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicAPI.Migrations
{
    public partial class AddTimePropToSongs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdioUrl",
                table: "Songs",
                newName: "AudioUrl");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "Songs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "Songs");

            migrationBuilder.RenameColumn(
                name: "AudioUrl",
                table: "Songs",
                newName: "AdioUrl");
        }
    }
}
