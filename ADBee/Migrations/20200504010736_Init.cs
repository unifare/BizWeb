using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADBee.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdStastics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad_Uuid = table.Column<string>(maxLength: 30, nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    TimeStartedDate = table.Column<DateTime>(nullable: false),
                    Click_Time = table.Column<DateTime>(nullable: false),
                    Ip4Client = table.Column<string>(nullable: true),
                    Ip4_1 = table.Column<int>(nullable: false),
                    Ip4_2 = table.Column<int>(nullable: false),
                    Ip4_3 = table.Column<int>(nullable: false),
                    Ip4_4 = table.Column<int>(nullable: false),
                    Ip6Client = table.Column<string>(nullable: true),
                    RefUrl = table.Column<string>(nullable: true),
                    AddTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdStastics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad_Uuid = table.Column<string>(maxLength: 30, nullable: false),
                    PublishUser = table.Column<string>(nullable: true),
                    AdType = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    AdUrl = table.Column<string>(nullable: true),
                    AddTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdStastics");

            migrationBuilder.DropTable(
                name: "Advertisements");
        }
    }
}
