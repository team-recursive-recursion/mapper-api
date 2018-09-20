using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MapperApi.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiveUser",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveUser", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "LiveLocation",
                columns: table => new
                {
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    UserID = table.Column<Guid>(nullable: false),
                    PointRaw = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveLocation", x => new { x.CreatedAt, x.UserID });
                    table.ForeignKey(
                        name: "FK_LiveLocation_LiveUser_UserID",
                        column: x => x.UserID,
                        principalTable: "LiveUser",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveLocation_UserID",
                table: "LiveLocation",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiveLocation");

            migrationBuilder.DropTable(
                name: "LiveUser");
        }
    }
}
