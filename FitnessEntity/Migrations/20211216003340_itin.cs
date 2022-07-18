using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessEntity.Migrations
{
    public partial class itin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    ThecoachId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    DanceTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DanceType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClassTypesInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    Type = table.Column<string>(type: "varchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTypesInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EntertainmentInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    Coursename = table.Column<string>(type: "varchar(36)", nullable: true),
                    CourseId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Standard = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThecoachInInfoId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntertainmentInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GoodsInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    GoodsName = table.Column<string>(type: "varchar(36)", nullable: true),
                    Num = table.Column<string>(type: "varchar(36)", nullable: true),
                    Money = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(8)", nullable: true),
                    GoodsTypeId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Description = table.Column<string>(type: "varchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GoodsRecord",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    GoodsId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    Num = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsRecord", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GoodsType",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(32)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MenuInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    Title = table.Column<string>(type: "varchar(16)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(32)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Href = table.Column<string>(type: "varchar(128)", nullable: true),
                    ParentId = table.Column<string>(type: "varchar(36)", nullable: true),
                    Icon = table.Column<string>(type: "varchar(32)", nullable: true),
                    Target = table.Column<string>(type: "varchar(16)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "R_RoleInfo_MenuInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    MenuID = table.Column<string>(type: "varchar(36)", nullable: true),
                    RoleID = table.Column<string>(type: "varchar(36)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_RoleInfo_MenuInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "R_UserInfo_RoleInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    UserID = table.Column<string>(type: "varchar(36)", nullable: true),
                    RoleID = table.Column<string>(type: "varchar(36)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_UserInfo_RoleInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RechargeInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    CurrentBalance = table.Column<string>(type: "varchar(36)", nullable: true),
                    InsertType = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RechargeInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RoleInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(16)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(36)", nullable: false),
                    Account = table.Column<string>(type: "varchar(16)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    PhoneNum = table.Column<string>(type: "varchar(16)", nullable: true),
                    Email = table.Column<string>(type: "varchar(32)", nullable: true),
                    NumberId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    PassWord = table.Column<string>(type: "varchar(32)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceInfo");

            migrationBuilder.DropTable(
                name: "ClassTypesInfo");

            migrationBuilder.DropTable(
                name: "EntertainmentInfo");

            migrationBuilder.DropTable(
                name: "GoodsInfo");

            migrationBuilder.DropTable(
                name: "GoodsRecord");

            migrationBuilder.DropTable(
                name: "GoodsType");

            migrationBuilder.DropTable(
                name: "MenuInfo");

            migrationBuilder.DropTable(
                name: "R_RoleInfo_MenuInfo");

            migrationBuilder.DropTable(
                name: "R_UserInfo_RoleInfo");

            migrationBuilder.DropTable(
                name: "RechargeInfo");

            migrationBuilder.DropTable(
                name: "RoleInfo");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
