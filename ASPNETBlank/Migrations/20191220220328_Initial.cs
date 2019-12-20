using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPNETBlank.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlInfos",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "VARCHAR(6)", nullable: false),
                    Url = table.Column<string>(type: "VARCHAR(2048)", nullable: false),
                    CreatonTime = table.Column<DateTime>(nullable: false),
                    UsesCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlInfos", x => x.Hash);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlInfos");
        }
    }
}
