using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class Updateallmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemDescription",
                table: "StoreGoodsStock");

            migrationBuilder.RenameColumn(
                name: "ItemDescription",
                table: "StoreGReceiveDetails",
                newName: "ItemName");

            migrationBuilder.RenameColumn(
                name: "ItemDescription",
                table: "StoreGIssueDetails",
                newName: "ItemName");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCode",
                table: "StoreGoodsStock",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "StoreGoodsStock",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "StoreGoodsStock");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "StoreGReceiveDetails",
                newName: "ItemDescription");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "StoreGIssueDetails",
                newName: "ItemDescription");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCode",
                table: "StoreGoodsStock",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                table: "StoreGoodsStock",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
