using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class recipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RMMaster",
                columns: table => new
                {
                    RMMId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RMItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RMItemName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RMMaster", x => x.RMMId);
                });

            migrationBuilder.CreateTable(
                name: "RMDetails",
                columns: table => new
                {
                    RMDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIGItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SIGItemName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SIGUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SGSUPrice = table.Column<float>(type: "real", nullable: false),
                    RMDQty = table.Column<int>(type: "int", nullable: false),
                    RMMId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RMDetails", x => x.RMDId);
                    table.ForeignKey(
                        name: "FK_RMDetails_RMMaster_RMMId",
                        column: x => x.RMMId,
                        principalTable: "RMMaster",
                        principalColumn: "RMMId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RMDetails_RMMId",
                table: "RMDetails",
                column: "RMMId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RMDetails");

            migrationBuilder.DropTable(
                name: "RMMaster");
        }
    }
}
