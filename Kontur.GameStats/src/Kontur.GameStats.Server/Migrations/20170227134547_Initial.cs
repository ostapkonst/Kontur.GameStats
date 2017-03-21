using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontur.GameStats.Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    endpoint = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameModes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    ServerModelId = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameModes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameModes_Servers_ServerModelId",
                        column: x => x.ServerModelId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    ServerModelId = table.Column<int>(nullable: false),
                    fragLimit = table.Column<uint>(nullable: false),
                    gameMode = table.Column<string>(nullable: true),
                    map = table.Column<string>(nullable: true),
                    timeElapsed = table.Column<double>(nullable: false),
                    timeLimit = table.Column<uint>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Servers_ServerModelId",
                        column: x => x.ServerModelId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreBoards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    MatcheModelId = table.Column<int>(nullable: false),
                    deaths = table.Column<uint>(nullable: false),
                    frags = table.Column<uint>(nullable: false),
                    kills = table.Column<uint>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    place = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreBoards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreBoards_Matches_MatcheModelId",
                        column: x => x.MatcheModelId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameModes_ServerModelId",
                table: "GameModes",
                column: "ServerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ServerModelId",
                table: "Matches",
                column: "ServerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreBoards_MatcheModelId",
                table: "ScoreBoards",
                column: "MatcheModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameModes");

            migrationBuilder.DropTable(
                name: "ScoreBoards");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
