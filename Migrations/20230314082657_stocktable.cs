using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class stocktable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreGoodsStock_StoreIGen_SIGId",
                table: "StoreGoodsStock");

            migrationBuilder.DropIndex(
                name: "IX_StoreGoodsStock_SIGId",
                table: "StoreGoodsStock");

            migrationBuilder.DropColumn(
                name: "SIGId",
                table: "StoreGoodsStock");

            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                table: "StoreGoodsStock",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                table: "StoreGoodsStock",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCode",
                table: "StoreGoodsStock");

            migrationBuilder.DropColumn(
                name: "ItemDescription",
                table: "StoreGoodsStock");

            migrationBuilder.AddColumn<int>(
                name: "SIGId",
                table: "StoreGoodsStock",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StoreGoodsStock_SIGId",
                table: "StoreGoodsStock",
                column: "SIGId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreGoodsStock_StoreIGen_SIGId",
                table: "StoreGoodsStock",
                column: "SIGId",
                principalTable: "StoreIGen",
                principalColumn: "SIGId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
