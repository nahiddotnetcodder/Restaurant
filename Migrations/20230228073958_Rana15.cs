using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class Rana15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AJTAccDescription",
                table: "AccGlTrans");

            migrationBuilder.AddColumn<string>(
                name: "AGTAccDescription",
                table: "AccGlTrans",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AGTAccDescription",
                table: "AccGlTrans");

            migrationBuilder.AddColumn<int>(
                name: "AJTAccDescription",
                table: "AccGlTrans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
