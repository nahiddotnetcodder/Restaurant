using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class ResInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResInfo",
                columns: table => new
                {
                    ResId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    REmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RSCharge = table.Column<int>(type: "int", nullable: false),
                    RTax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResInfo", x => x.ResId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResInfo");
        }
    }
}
