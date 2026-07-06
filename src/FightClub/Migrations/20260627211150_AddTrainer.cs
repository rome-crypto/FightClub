using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FightClub.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrainerId",
                table: "Boxers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boxers_TrainerId",
                table: "Boxers",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxers_Trainers_TrainerId",
                table: "Boxers",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxers_Trainers_TrainerId",
                table: "Boxers");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Boxers_TrainerId",
                table: "Boxers");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Boxers");
        }
    }
}
