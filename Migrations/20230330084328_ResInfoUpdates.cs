using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class ResInfoUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RPhotoUrl",
                table: "ResInfo",
                newName: "RCLogoUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RCLogoUrl",
                table: "ResInfo",
                newName: "RPhotoUrl");
        }
    }
}
