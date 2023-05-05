using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSM.Migrations
{
    /// <inheritdoc />
    public partial class InitialMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GamesData",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    ChatGPTDescription = table.Column<string>(type: "longtext", nullable: true),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesData", x => x.Name);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameServerStats",
                columns: table => new
                {
                    GameName = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PlayersCount = table.Column<long>(type: "bigint", nullable: false),
                    CPUUsage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RAMUsage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MAXCPU = table.Column<float>(type: "float", nullable: false),
                    RAMSize = table.Column<float>(type: "float", nullable: false),
                    TopScore = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameServerStats", x => new { x.GameName, x.UpdateDate });
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    Genre = table.Column<string>(type: "varchar(255)", nullable: false),
                    GameName = table.Column<string>(type: "varchar(255)", nullable: false),
                    GameDataName = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => new { x.Genre, x.GameName });
                    table.ForeignKey(
                        name: "FK_GameGenre_GamesData_GameDataName",
                        column: x => x.GameDataName,
                        principalTable: "GamesData",
                        principalColumn: "Name");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GameDataName",
                table: "GameGenre",
                column: "GameDataName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGenre");

            migrationBuilder.DropTable(
                name: "GameServerStats");

            migrationBuilder.DropTable(
                name: "GamesData");
        }
    }
}
