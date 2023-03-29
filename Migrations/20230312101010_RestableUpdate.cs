using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class RestableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResMenu",
                columns: table => new
                {
                    RMId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RKId = table.Column<int>(type: "int", nullable: false),
                    RFTId = table.Column<int>(type: "int", nullable: false),
                    RMItemCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RMItemName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RMUPrice = table.Column<double>(type: "float", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResMenu", x => x.RMId);
                    table.ForeignKey(
                        name: "FK_ResMenu_ResFoodType_RFTId",
                        column: x => x.RFTId,
                        principalTable: "ResFoodType",
                        principalColumn: "RFTId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResMenu_ResKitchenInfo_RKId",
                        column: x => x.RKId,
                        principalTable: "ResKitchenInfo",
                        principalColumn: "RKId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResMenu_RFTId",
                table: "ResMenu",
                column: "RFTId");

            migrationBuilder.CreateIndex(
                name: "IX_ResMenu_RKId",
                table: "ResMenu",
                column: "RKId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResMenu");
        }
    }
}
