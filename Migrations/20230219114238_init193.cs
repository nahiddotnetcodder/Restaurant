using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class init193 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HREmpDetails_HRSalary_HREmpSalaryHRSId",
                table: "HREmpDetails");

            migrationBuilder.DropIndex(
                name: "IX_HREmpDetails_HREmpSalaryHRSId",
                table: "HREmpDetails");

            migrationBuilder.DropColumn(
                name: "HREmpSalaryHRSId",
                table: "HREmpDetails");

            migrationBuilder.AlterColumn<string>(
                name: "SSEmail",
                table: "StoreSupplier",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SSBName",
                table: "StoreSupplier",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SSEmail",
                table: "StoreSupplier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SSBName",
                table: "StoreSupplier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HREmpSalaryHRSId",
                table: "HREmpDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HREmpDetails_HREmpSalaryHRSId",
                table: "HREmpDetails",
                column: "HREmpSalaryHRSId");

            migrationBuilder.AddForeignKey(
                name: "FK_HREmpDetails_HRSalary_HREmpSalaryHRSId",
                table: "HREmpDetails",
                column: "HREmpSalaryHRSId",
                principalTable: "HRSalary",
                principalColumn: "HRSId");
        }
    }
}
