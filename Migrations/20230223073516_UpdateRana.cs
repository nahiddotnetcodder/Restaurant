using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class UpdateRana : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AGTAmount",
                table: "AccGlTrans");

            migrationBuilder.DropColumn(
                name: "AJTrDate",
                table: "AccGlTrans");

            migrationBuilder.RenameColumn(
                name: "AJType",
                table: "AccGlTrans",
                newName: "AJTAccDescription");

            migrationBuilder.RenameColumn(
                name: "AGTAccount",
                table: "AccGlTrans",
                newName: "AGTAccCode");

            migrationBuilder.AddColumn<string>(
                name: "AJType",
                table: "AccJournal",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AGTCreditAccount",
                table: "AccGlTrans",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AGTDebitAccount",
                table: "AccGlTrans",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AJType",
                table: "AccJournal");

            migrationBuilder.DropColumn(
                name: "AGTCreditAccount",
                table: "AccGlTrans");

            migrationBuilder.DropColumn(
                name: "AGTDebitAccount",
                table: "AccGlTrans");

            migrationBuilder.RenameColumn(
                name: "AJTAccDescription",
                table: "AccGlTrans",
                newName: "AJType");

            migrationBuilder.RenameColumn(
                name: "AGTAccCode",
                table: "AccGlTrans",
                newName: "AGTAccount");

            migrationBuilder.AddColumn<double>(
                name: "AGTAmount",
                table: "AccGlTrans",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "AJTrDate",
                table: "AccGlTrans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
