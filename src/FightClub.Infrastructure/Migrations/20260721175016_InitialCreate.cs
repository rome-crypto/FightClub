using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FightClub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boxers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    TrainerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Statistics_Wins = table.Column<int>(type: "integer", nullable: false),
                    Statistics_Losses = table.Column<int>(type: "integer", nullable: false),
                    Statistics_Draws = table.Column<int>(type: "integer", nullable: false),
                    Statistics_Knockouts = table.Column<int>(type: "integer", nullable: false),
                    Statistics_TechnicalKnockouts = table.Column<int>(type: "integer", nullable: false),
                    Statistics_KnockoutLosses = table.Column<int>(type: "integer", nullable: false),
                    Statistics_TechnicalKnockoutLosses = table.Column<int>(type: "integer", nullable: false),
                    Statistics_WinStreak = table.Column<int>(type: "integer", nullable: false),
                    Statistics_BestWinStreak = table.Column<int>(type: "integer", nullable: false),
                    Statistics_LastFightDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ranking_EloRating = table.Column<int>(type: "integer", nullable: false),
                    Ranking_RankingPoints = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boxers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BoxerAId = table.Column<Guid>(type: "uuid", nullable: false),
                    BoxerBId = table.Column<Guid>(type: "uuid", nullable: false),
                    WinnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EndType = table.Column<int>(type: "integer", nullable: true),
                    PlannedRounds = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FightDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FightRound",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    ScoreA = table.Column<int>(type: "integer", nullable: false),
                    ScoreB = table.Column<int>(type: "integer", nullable: false),
                    IsFinished = table.Column<bool>(type: "boolean", nullable: false),
                    FightId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FightRound", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FightRound_Fights_FightId",
                        column: x => x.FightId,
                        principalTable: "Fights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoundEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    BoxerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FightRoundId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoundEvent_FightRound_FightRoundId",
                        column: x => x.FightRoundId,
                        principalTable: "FightRound",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FightRound_FightId",
                table: "FightRound",
                column: "FightId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundEvent_FightRoundId",
                table: "RoundEvent",
                column: "FightRoundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boxers");

            migrationBuilder.DropTable(
                name: "RoundEvent");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "FightRound");

            migrationBuilder.DropTable(
                name: "Fights");
        }
    }
}
