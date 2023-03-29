using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccBankAccounts",
                columns: table => new
                {
                    ABAId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACMAccCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ABAAType = table.Column<int>(type: "int", nullable: false),
                    ABABAName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ABABANumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ABABName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ABABAdd = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ABABCCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ABALRDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ABAERBal = table.Column<double>(type: "float", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccBankAccounts", x => x.ABAId);
                });

            migrationBuilder.CreateTable(
                name: "AccChartClass",
                columns: table => new
                {
                    ACCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    ACCName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ACCCType = table.Column<int>(type: "int", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccChartClass", x => x.ACCId);
                });

            migrationBuilder.CreateTable(
                name: "AccFiscalYear",
                columns: table => new
                {
                    AFYId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AFYBeginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AFYEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AFYClosed = table.Column<int>(type: "int", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccFiscalYear", x => x.AFYId);
                });

            migrationBuilder.CreateTable(
                name: "AccJournal",
                columns: table => new
                {
                    AJId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AJTrDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AJRefPrefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AJRef = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AJSoRef = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AJEDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AJDDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AJAmount = table.Column<double>(type: "float", nullable: false),
                    AJMemo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccJournal", x => x.AJId);
                });

            migrationBuilder.CreateTable(
                name: "AccPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MenuItem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccPermission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    CurrentStatusId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserLogin = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    AccessLevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRDepartment",
                columns: table => new
                {
                    HRDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRDName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HRDDes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDepartment", x => x.HRDId);
                });

            migrationBuilder.CreateTable(
                name: "HRDesignation",
                columns: table => new
                {
                    HRDeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRDeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HRDeDes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDesignation", x => x.HRDeId);
                });

            migrationBuilder.CreateTable(
                name: "HRHolidays",
                columns: table => new
                {
                    HRHId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayType = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    HRHStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HRHEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRHolidays", x => x.HRHId);
                });

            migrationBuilder.CreateTable(
                name: "HRSalaryPolicy",
                columns: table => new
                {
                    HRSPId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRSPName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ADDUC = table.Column<int>(type: "int", nullable: false),
                    PerNPer = table.Column<int>(type: "int", nullable: false),
                    HRSPPercent = table.Column<float>(type: "real", nullable: true),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRSalaryPolicy", x => x.HRSPId);
                });

            migrationBuilder.CreateTable(
                name: "HRWeekend",
                columns: table => new
                {
                    HRWId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weekday = table.Column<int>(type: "int", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRWeekend", x => x.HRWId);
                });

            migrationBuilder.CreateTable(
                name: "HRWStatus",
                columns: table => new
                {
                    HRWSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRWSName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HRWSDes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRWStatus", x => x.HRWSId);
                });

            migrationBuilder.CreateTable(
                name: "StoreCategory",
                columns: table => new
                {
                    SCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCategory", x => x.SCId);
                });

            migrationBuilder.CreateTable(
                name: "StoreDClose",
                columns: table => new
                {
                    SDCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SDCDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreDClose", x => x.SDCId);
                });

            migrationBuilder.CreateTable(
                name: "StoreGReceiveMasters",
                columns: table => new
                {
                    GRMId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GRMDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GRMSupplier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GRMRemarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreGReceiveMasters", x => x.GRMId);
                });

            migrationBuilder.CreateTable(
                name: "StoreUnit",
                columns: table => new
                {
                    SUId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreUnit", x => x.SUId);
                });

            migrationBuilder.CreateTable(
                name: "AccChartType",
                columns: table => new
                {
                    ACTId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACTClassId = table.Column<int>(type: "int", nullable: false),
                    ACTName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ACCId = table.Column<int>(type: "int", nullable: true),
                    ACTParentId = table.Column<int>(type: "int", nullable: true),
                    ACTParentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccChartType", x => x.ACTId);
                    table.ForeignKey(
                        name: "FK_AccChartType_AccChartClass_ACCId",
                        column: x => x.ACCId,
                        principalTable: "AccChartClass",
                        principalColumn: "ACCId");
                });

            migrationBuilder.CreateTable(
                name: "AccGlTrans",
                columns: table => new
                {
                    AGTId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AJType = table.Column<int>(type: "int", nullable: false),
                    AJTrNo = table.Column<int>(type: "int", nullable: false),
                    AJTrDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AGTAccount = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AGTMemo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AGTAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccGlTrans", x => x.AGTId);
                    table.ForeignKey(
                        name: "FK_AccGlTrans_AccJournal_AJTrNo",
                        column: x => x.AJTrNo,
                        principalTable: "AccJournal",
                        principalColumn: "AJId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreGIssueMasters",
                columns: table => new
                {
                    GIMId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GIMDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HRDId = table.Column<int>(type: "int", nullable: false),
                    GIMRemarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreGIssueMasters", x => x.GIMId);
                    table.ForeignKey(
                        name: "FK_StoreGIssueMasters_HRDepartment_HRDId",
                        column: x => x.HRDId,
                        principalTable: "HRDepartment",
                        principalColumn: "HRDId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreSCategory",
                columns: table => new
                {
                    SSCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCId = table.Column<int>(type: "int", nullable: false),
                    SSCName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSCategory", x => x.SSCId);
                    table.ForeignKey(
                        name: "FK_StoreSCategory_StoreCategory_SCId",
                        column: x => x.SCId,
                        principalTable: "StoreCategory",
                        principalColumn: "SCId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccChartMaster",
                columns: table => new
                {
                    ACMId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACMAccCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ACMAccName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ACTId = table.Column<int>(type: "int", nullable: false),
                    ACMAI = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccChartMaster", x => x.ACMId);
                    table.ForeignKey(
                        name: "FK_AccChartMaster_AccChartType_ACTId",
                        column: x => x.ACTId,
                        principalTable: "AccChartType",
                        principalColumn: "ACTId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreIGen",
                columns: table => new
                {
                    SIGId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCId = table.Column<int>(type: "int", nullable: false),
                    SSCId = table.Column<int>(type: "int", nullable: true),
                    SIGItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SIGItemName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SUId = table.Column<int>(type: "int", nullable: false),
                    SIGRLevel = table.Column<int>(type: "int", nullable: false),
                    SIGRemarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreIGen", x => x.SIGId);
                    table.ForeignKey(
                        name: "FK_StoreIGen_StoreCategory_SCId",
                        column: x => x.SCId,
                        principalTable: "StoreCategory",
                        principalColumn: "SCId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreIGen_StoreSCategory_SSCId",
                        column: x => x.SSCId,
                        principalTable: "StoreSCategory",
                        principalColumn: "SSCId");
                    table.ForeignKey(
                        name: "FK_StoreIGen_StoreUnit_SUId",
                        column: x => x.SUId,
                        principalTable: "StoreUnit",
                        principalColumn: "SUId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreGIssueDetails",
                columns: table => new
                {
                    GIDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIGId = table.Column<int>(type: "int", nullable: false),
                    GIDUPrice = table.Column<decimal>(type: "smallmoney", nullable: false),
                    GIDQty = table.Column<float>(type: "real", nullable: false),
                    GIDTPrice = table.Column<int>(type: "int", nullable: false),
                    GIMId = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreGIssueDetails", x => x.GIDId);
                    table.ForeignKey(
                        name: "FK_StoreGIssueDetails_StoreGIssueMasters_GIMId",
                        column: x => x.GIMId,
                        principalTable: "StoreGIssueMasters",
                        principalColumn: "GIMId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreGIssueDetails_StoreIGen_SIGId",
                        column: x => x.SIGId,
                        principalTable: "StoreIGen",
                        principalColumn: "SIGId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreGoodsStock",
                columns: table => new
                {
                    SGSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIGId = table.Column<int>(type: "int", nullable: false),
                    SGSQty = table.Column<float>(type: "real", nullable: false),
                    SGSUPrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreGoodsStock", x => x.SGSId);
                    table.ForeignKey(
                        name: "FK_StoreGoodsStock_StoreIGen_SIGId",
                        column: x => x.SIGId,
                        principalTable: "StoreIGen",
                        principalColumn: "SIGId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreGReceiveDetails",
                columns: table => new
                {
                    GRDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIGId = table.Column<int>(type: "int", nullable: false),
                    GRDQty = table.Column<float>(type: "real", nullable: false),
                    GRDUPrice = table.Column<decimal>(type: "smallmoney", nullable: false),
                    GRDTPrice = table.Column<int>(type: "int", nullable: false),
                    GRMId = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreGReceiveDetails", x => x.GRDId);
                    table.ForeignKey(
                        name: "FK_StoreGReceiveDetails_StoreGReceiveMasters_GRMId",
                        column: x => x.GRMId,
                        principalTable: "StoreGReceiveMasters",
                        principalColumn: "GRMId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreGReceiveDetails_StoreIGen_SIGId",
                        column: x => x.SIGId,
                        principalTable: "StoreIGen",
                        principalColumn: "SIGId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HREmpAtt",
                columns: table => new
                {
                    HREAId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HREDId = table.Column<int>(type: "int", nullable: false),
                    HREADate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HREAInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HREAOutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HREATMinute = table.Column<int>(type: "int", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HREmpAtt", x => x.HREAId);
                });

            migrationBuilder.CreateTable(
                name: "HREmpDetails",
                columns: table => new
                {
                    HREDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRDId = table.Column<int>(type: "int", nullable: false),
                    HRDeId = table.Column<int>(type: "int", nullable: false),
                    HREDEId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HREDEName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    HREDFName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    HREDPreAdd = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    HREDParAdd = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    HREDCont = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HREDBGroup = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HREDNat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HREDReligion = table.Column<int>(type: "int", nullable: false),
                    HREDMStatus = table.Column<int>(type: "int", nullable: false),
                    HREDRef = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    HREDRAdd = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    HREDJDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HRWSId = table.Column<int>(type: "int", nullable: false),
                    HREDEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HREDBasic = table.Column<float>(type: "real", nullable: false),
                    IsWaiter = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    HREDPUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HREmpSalaryHRSId = table.Column<int>(type: "int", nullable: true),
                    HRLeaveDetailHRLDId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HREmpDetails", x => x.HREDId);
                    table.ForeignKey(
                        name: "FK_HREmpDetails_HRDepartment_HRDId",
                        column: x => x.HRDId,
                        principalTable: "HRDepartment",
                        principalColumn: "HRDId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HREmpDetails_HRDesignation_HRDeId",
                        column: x => x.HRDeId,
                        principalTable: "HRDesignation",
                        principalColumn: "HRDeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HREmpDetails_HRWStatus_HRWSId",
                        column: x => x.HRWSId,
                        principalTable: "HRWStatus",
                        principalColumn: "HRWSId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HREmpRoaster",
                columns: table => new
                {
                    HRERId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HREDId = table.Column<int>(type: "int", nullable: true),
                    HRERFDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HRERTDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftType = table.Column<int>(type: "int", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HREmpRoaster", x => x.HRERId);
                    table.ForeignKey(
                        name: "FK_HREmpRoaster_HREmpDetails_HREDId",
                        column: x => x.HREDId,
                        principalTable: "HREmpDetails",
                        principalColumn: "HREDId");
                });

            migrationBuilder.CreateTable(
                name: "HRSalary",
                columns: table => new
                {
                    HRSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRSYear = table.Column<int>(type: "int", nullable: false),
                    HRSMonth = table.Column<int>(type: "int", nullable: false),
                    HREDId = table.Column<int>(type: "int", nullable: false),
                    HRSBasic = table.Column<double>(type: "float", nullable: false),
                    HRSGTotal = table.Column<double>(type: "float", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRSalary", x => x.HRSId);
                    table.ForeignKey(
                        name: "FK_HRSalary_HREmpDetails_HREDId",
                        column: x => x.HREDId,
                        principalTable: "HREmpDetails",
                        principalColumn: "HREDId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRLeaveDetail",
                columns: table => new
                {
                    HRLDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HREDId = table.Column<int>(type: "int", nullable: false),
                    HRLPId = table.Column<int>(type: "int", nullable: false),
                    HRLDAppSl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HRLDAppDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HRLDLeaveSDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HRLDLeaveEDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HRLDReason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    HREDIdSu = table.Column<int>(type: "int", nullable: false),
                    HRLDTDay = table.Column<int>(type: "int", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRLeaveDetail", x => x.HRLDId);
                    table.ForeignKey(
                        name: "FK_HRLeaveDetail_HREmpDetails_HREDId",
                        column: x => x.HREDId,
                        principalTable: "HREmpDetails",
                        principalColumn: "HREDId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRLeavePolicy",
                columns: table => new
                {
                    HRLPId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRLPName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HRLPTDay = table.Column<int>(type: "int", nullable: false),
                    CUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HRLeaveDetailHRLDId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRLeavePolicy", x => x.HRLPId);
                    table.ForeignKey(
                        name: "FK_HRLeavePolicy_HRLeaveDetail_HRLeaveDetailHRLDId",
                        column: x => x.HRLeaveDetailHRLDId,
                        principalTable: "HRLeaveDetail",
                        principalColumn: "HRLDId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccChartMaster_ACTId",
                table: "AccChartMaster",
                column: "ACTId");

            migrationBuilder.CreateIndex(
                name: "IX_AccChartType_ACCId",
                table: "AccChartType",
                column: "ACCId");

            migrationBuilder.CreateIndex(
                name: "IX_AccGlTrans_AJTrNo",
                table: "AccGlTrans",
                column: "AJTrNo");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HREmpAtt_HREDId",
                table: "HREmpAtt",
                column: "HREDId");

            migrationBuilder.CreateIndex(
                name: "IX_HREmpDetails_HRDeId",
                table: "HREmpDetails",
                column: "HRDeId");

            migrationBuilder.CreateIndex(
                name: "IX_HREmpDetails_HRDId",
                table: "HREmpDetails",
                column: "HRDId");

            migrationBuilder.CreateIndex(
                name: "IX_HREmpDetails_HREmpSalaryHRSId",
                table: "HREmpDetails",
                column: "HREmpSalaryHRSId");

            migrationBuilder.CreateIndex(
                name: "IX_HREmpDetails_HRLeaveDetailHRLDId",
                table: "HREmpDetails",
                column: "HRLeaveDetailHRLDId");

            migrationBuilder.CreateIndex(
                name: "IX_HREmpDetails_HRWSId",
                table: "HREmpDetails",
                column: "HRWSId");

            migrationBuilder.CreateIndex(
                name: "IX_HREmpRoaster_HREDId",
                table: "HREmpRoaster",
                column: "HREDId");

            migrationBuilder.CreateIndex(
                name: "IX_HRLeaveDetail_HREDId",
                table: "HRLeaveDetail",
                column: "HREDId");

            migrationBuilder.CreateIndex(
                name: "IX_HRLeaveDetail_HRLPId",
                table: "HRLeaveDetail",
                column: "HRLPId");

            migrationBuilder.CreateIndex(
                name: "IX_HRLeavePolicy_HRLeaveDetailHRLDId",
                table: "HRLeavePolicy",
                column: "HRLeaveDetailHRLDId");

            migrationBuilder.CreateIndex(
                name: "IX_HRSalary_HREDId",
                table: "HRSalary",
                column: "HREDId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreGIssueDetails_GIMId",
                table: "StoreGIssueDetails",
                column: "GIMId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreGIssueDetails_SIGId",
                table: "StoreGIssueDetails",
                column: "SIGId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreGIssueMasters_HRDId",
                table: "StoreGIssueMasters",
                column: "HRDId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreGoodsStock_SIGId",
                table: "StoreGoodsStock",
                column: "SIGId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreGReceiveDetails_GRMId",
                table: "StoreGReceiveDetails",
                column: "GRMId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreGReceiveDetails_SIGId",
                table: "StoreGReceiveDetails",
                column: "SIGId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreIGen_SCId",
                table: "StoreIGen",
                column: "SCId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreIGen_SSCId",
                table: "StoreIGen",
                column: "SSCId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreIGen_SUId",
                table: "StoreIGen",
                column: "SUId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreSCategory_SCId",
                table: "StoreSCategory",
                column: "SCId");

            migrationBuilder.AddForeignKey(
                name: "FK_HREmpAtt_HREmpDetails_HREDId",
                table: "HREmpAtt",
                column: "HREDId",
                principalTable: "HREmpDetails",
                principalColumn: "HREDId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HREmpDetails_HRLeaveDetail_HRLeaveDetailHRLDId",
                table: "HREmpDetails",
                column: "HRLeaveDetailHRLDId",
                principalTable: "HRLeaveDetail",
                principalColumn: "HRLDId");

            migrationBuilder.AddForeignKey(
                name: "FK_HREmpDetails_HRSalary_HREmpSalaryHRSId",
                table: "HREmpDetails",
                column: "HREmpSalaryHRSId",
                principalTable: "HRSalary",
                principalColumn: "HRSId");

            migrationBuilder.AddForeignKey(
                name: "FK_HRLeaveDetail_HRLeavePolicy_HRLPId",
                table: "HRLeaveDetail",
                column: "HRLPId",
                principalTable: "HRLeavePolicy",
                principalColumn: "HRLPId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HRLeaveDetail_HREmpDetails_HREDId",
                table: "HRLeaveDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_HRSalary_HREmpDetails_HREDId",
                table: "HRSalary");

            migrationBuilder.DropForeignKey(
                name: "FK_HRLeavePolicy_HRLeaveDetail_HRLeaveDetailHRLDId",
                table: "HRLeavePolicy");

            migrationBuilder.DropTable(
                name: "AccBankAccounts");

            migrationBuilder.DropTable(
                name: "AccChartMaster");

            migrationBuilder.DropTable(
                name: "AccFiscalYear");

            migrationBuilder.DropTable(
                name: "AccGlTrans");

            migrationBuilder.DropTable(
                name: "AccPermission");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HREmpAtt");

            migrationBuilder.DropTable(
                name: "HREmpRoaster");

            migrationBuilder.DropTable(
                name: "HRHolidays");

            migrationBuilder.DropTable(
                name: "HRSalaryPolicy");

            migrationBuilder.DropTable(
                name: "HRWeekend");

            migrationBuilder.DropTable(
                name: "StoreDClose");

            migrationBuilder.DropTable(
                name: "StoreGIssueDetails");

            migrationBuilder.DropTable(
                name: "StoreGoodsStock");

            migrationBuilder.DropTable(
                name: "StoreGReceiveDetails");

            migrationBuilder.DropTable(
                name: "AccChartType");

            migrationBuilder.DropTable(
                name: "AccJournal");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "StoreGIssueMasters");

            migrationBuilder.DropTable(
                name: "StoreGReceiveMasters");

            migrationBuilder.DropTable(
                name: "StoreIGen");

            migrationBuilder.DropTable(
                name: "AccChartClass");

            migrationBuilder.DropTable(
                name: "StoreSCategory");

            migrationBuilder.DropTable(
                name: "StoreUnit");

            migrationBuilder.DropTable(
                name: "StoreCategory");

            migrationBuilder.DropTable(
                name: "HREmpDetails");

            migrationBuilder.DropTable(
                name: "HRDepartment");

            migrationBuilder.DropTable(
                name: "HRDesignation");

            migrationBuilder.DropTable(
                name: "HRSalary");

            migrationBuilder.DropTable(
                name: "HRWStatus");

            migrationBuilder.DropTable(
                name: "HRLeaveDetail");

            migrationBuilder.DropTable(
                name: "HRLeavePolicy");
        }
    }
}
