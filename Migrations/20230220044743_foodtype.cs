using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class foodtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResFoodType",
                columns: table => new
                {
                    RFTId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFTName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RFTDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResFoodType", x => x.RFTId);
                });

            migrationBuilder.CreateTable(
                name: "ResKitchenInfo",
                columns: table => new
                {
                    RKId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RKitchenName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RKDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResKitchenInfo", x => x.RKId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResFoodType");

            migrationBuilder.DropTable(
                name: "ResKitchenInfo");
        }
    }
}
