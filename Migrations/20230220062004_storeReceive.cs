using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class storeReceive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GRMSupplier",
                table: "StoreGReceiveMasters");

            migrationBuilder.AddColumn<int>(
                name: "SSId",
                table: "StoreGReceiveMasters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StoreGReceiveMasters_SSId",
                table: "StoreGReceiveMasters",
                column: "SSId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreGReceiveMasters_StoreSupplier_SSId",
                table: "StoreGReceiveMasters",
                column: "SSId",
                principalTable: "StoreSupplier",
                principalColumn: "SSId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreGReceiveMasters_StoreSupplier_SSId",
                table: "StoreGReceiveMasters");

            migrationBuilder.DropIndex(
                name: "IX_StoreGReceiveMasters_SSId",
                table: "StoreGReceiveMasters");

            migrationBuilder.DropColumn(
                name: "SSId",
                table: "StoreGReceiveMasters");

            migrationBuilder.AddColumn<string>(
                name: "GRMSupplier",
                table: "StoreGReceiveMasters",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
