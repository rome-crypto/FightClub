using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FightClub.Migrations
{
    /// <inheritdoc />
    public partial class AddBoxerStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Draws",
                table: "Boxers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Losses",
                table: "Boxers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "Boxers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BoxerAId = table.Column<Guid>(type: "uuid", nullable: false),
                    BoxerBId = table.Column<Guid>(type: "uuid", nullable: false),
                    WinnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    FightDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Rounds = table.Column<int>(type: "integer", nullable: false),
                    ResultMethod = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Boxers_BoxerAId",
                        column: x => x.BoxerAId,
                        principalTable: "Boxers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Boxers_BoxerBId",
                        column: x => x.BoxerBId,
                        principalTable: "Boxers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Boxers_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Boxers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_BoxerAId",
                table: "Matches",
                column: "BoxerAId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_BoxerBId",
                table: "Matches",
                column: "BoxerBId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_WinnerId",
                table: "Matches",
                column: "WinnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropColumn(
                name: "Draws",
                table: "Boxers");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "Boxers");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "Boxers");
        }
    }
}
