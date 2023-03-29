using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreSupplier",
                columns: table => new
                {
                    SSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SSName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SSSName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SSOAdd = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SSCPerson = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SSCNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SSEmail = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    SSBName = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    SSGNotes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ACMId = table.Column<int>(type: "int", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSupplier", x => x.SSId);
                    table.ForeignKey(
                        name: "FK_StoreSupplier_AccChartMaster_ACMId",
                        column: x => x.ACMId,
                        principalTable: "AccChartMaster",
                        principalColumn: "ACMId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreSupplier_ACMId",
                table: "StoreSupplier",
                column: "ACMId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreSupplier");
        }
    }
}
