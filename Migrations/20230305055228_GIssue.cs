using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class GIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreGIssueDetails_StoreIGen_SIGId",
                table: "StoreGIssueDetails");

            migrationBuilder.DropIndex(
                name: "IX_StoreGIssueDetails_SIGId",
                table: "StoreGIssueDetails");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "StoreGIssueDetails");

            migrationBuilder.DropColumn(
                name: "SIGId",
                table: "StoreGIssueDetails");

            migrationBuilder.AddColumn<string>(
                name: "ItemCode",
                table: "StoreGIssueDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                table: "StoreGIssueDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCode",
                table: "StoreGIssueDetails");

            migrationBuilder.DropColumn(
                name: "ItemDescription",
                table: "StoreGIssueDetails");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "StoreGIssueDetails",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SIGId",
                table: "StoreGIssueDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StoreGIssueDetails_SIGId",
                table: "StoreGIssueDetails",
                column: "SIGId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreGIssueDetails_StoreIGen_SIGId",
                table: "StoreGIssueDetails",
                column: "SIGId",
                principalTable: "StoreIGen",
                principalColumn: "SIGId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
