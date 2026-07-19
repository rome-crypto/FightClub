using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FightClub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Trainers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Age = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Trainers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Boxers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Age = table.Column<int>(type: "integer", nullable: false),
                WeightCategory = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                TrainerId = table.Column<Guid>(type: "uuid", nullable: false),
                Wins = table.Column<int>(type: "integer", nullable: false),
                Draws = table.Column<int>(type: "integer", nullable: false),
                Losses = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Boxers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Boxers_Trainers_TrainerId",
                    column: x => x.TrainerId,
                    principalTable: "Trainers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "Fights",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                BoxerAId = table.Column<Guid>(type: "uuid", nullable: false),
                BoxerBId = table.Column<Guid>(type: "uuid", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                WinnerId = table.Column<Guid>(type: "uuid", nullable: true),
                PlannedRounds = table.Column<int>(type: "integer", nullable: false),
                BoxerId = table.Column<Guid>(type: "uuid", nullable: true),
                BoxerId1 = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Fights", x => x.Id);
                table.ForeignKey(
                    name: "FK_Fights_Boxers_BoxerAId",
                    column: x => x.BoxerAId,
                    principalTable: "Boxers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Fights_Boxers_BoxerBId",
                    column: x => x.BoxerBId,
                    principalTable: "Boxers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Fights_Boxers_BoxerId",
                    column: x => x.BoxerId,
                    principalTable: "Boxers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Fights_Boxers_BoxerId1",
                    column: x => x.BoxerId1,
                    principalTable: "Boxers",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "FightRound",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FightId = table.Column<Guid>(type: "uuid", nullable: false),
                Number = table.Column<int>(type: "integer", nullable: false),
                ScoreA = table.Column<int>(type: "integer", nullable: false),
                ScoreB = table.Column<int>(type: "integer", nullable: false),
                Result = table.Column<int>(type: "integer", nullable: false)
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
                FightRoundId = table.Column<Guid>(type: "uuid", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false),
                SourceBoxerId = table.Column<Guid>(type: "uuid", nullable: true),
                Impact = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
            name: "IX_Boxers_TrainerId",
            table: "Boxers",
            column: "TrainerId");

        migrationBuilder.CreateIndex(
            name: "IX_FightRound_FightId",
            table: "FightRound",
            column: "FightId");

        migrationBuilder.CreateIndex(
            name: "IX_Fights_BoxerAId",
            table: "Fights",
            column: "BoxerAId");

        migrationBuilder.CreateIndex(
            name: "IX_Fights_BoxerBId",
            table: "Fights",
            column: "BoxerBId");

        migrationBuilder.CreateIndex(
            name: "IX_Fights_BoxerId",
            table: "Fights",
            column: "BoxerId");

        migrationBuilder.CreateIndex(
            name: "IX_Fights_BoxerId1",
            table: "Fights",
            column: "BoxerId1");

        migrationBuilder.CreateIndex(
            name: "IX_RoundEvent_FightRoundId",
            table: "RoundEvent",
            column: "FightRoundId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RoundEvent");

        migrationBuilder.DropTable(
            name: "FightRound");

        migrationBuilder.DropTable(
            name: "Fights");

        migrationBuilder.DropTable(
            name: "Boxers");

        migrationBuilder.DropTable(
            name: "Trainers");
    }
}
