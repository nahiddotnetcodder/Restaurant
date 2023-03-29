using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreGReceiveDetails_StoreIGen_SIGId",
                table: "StoreGReceiveDetails");

            migrationBuilder.DropIndex(
                name: "IX_StoreGReceiveDetails_SIGId",
                table: "StoreGReceiveDetails");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "StoreGReceiveDetails");

            migrationBuilder.DropColumn(
                name: "SIGId",
                table: "StoreGReceiveDetails");

            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                table: "StoreGReceiveDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                table: "StoreGReceiveDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCode",
                table: "StoreGReceiveDetails");

            migrationBuilder.DropColumn(
                name: "ItemDescription",
                table: "StoreGReceiveDetails");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "StoreGReceiveDetails",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SIGId",
                table: "StoreGReceiveDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StoreGReceiveDetails_SIGId",
                table: "StoreGReceiveDetails",
                column: "SIGId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreGReceiveDetails_StoreIGen_SIGId",
                table: "StoreGReceiveDetails",
                column: "SIGId",
                principalTable: "StoreIGen",
                principalColumn: "SIGId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
