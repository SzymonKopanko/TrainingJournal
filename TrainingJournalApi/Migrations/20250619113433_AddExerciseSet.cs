using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingJournalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseEntryId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Reps = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    OneRepMax = table.Column<double>(type: "float", nullable: false),
                    RIR = table.Column<int>(type: "int", nullable: false),
                    PercievedOneRepMax = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseSets_ExerciseEntries_ExerciseEntryId",
                        column: x => x.ExerciseEntryId,
                        principalTable: "ExerciseEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "d21116a6-f3fb-425d-8c9e-66143eedf3f1", new DateTime(2025, 6, 19, 11, 34, 33, 96, DateTimeKind.Utc).AddTicks(9118), "AQAAAAIAAYagAAAAEKqsMXrUXmXuN2NvaYlcenSldf8cApc0m2Pmqm8rV3nRYNw2iBWfYf60CGvPfKPfqA==" });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 11, 34, 33, 152, DateTimeKind.Utc).AddTicks(6407));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 11, 34, 33, 152, DateTimeKind.Utc).AddTicks(6409));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 11, 34, 33, 152, DateTimeKind.Utc).AddTicks(6411));

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSets_ExerciseEntryId",
                table: "ExerciseSets",
                column: "ExerciseEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseSets");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "473a4b00-fb1e-458a-92cb-65804512e2ee", new DateTime(2025, 6, 19, 11, 25, 3, 396, DateTimeKind.Utc).AddTicks(9054), "AQAAAAIAAYagAAAAECH40MTDg5Nf54+Drdb0c4Tl6qANPbQ94z944M/DvpjxZ31JlHESr2iQdzbGPQ7e6Q==" });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 11, 25, 3, 458, DateTimeKind.Utc).AddTicks(5313));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 11, 25, 3, 458, DateTimeKind.Utc).AddTicks(5315));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 11, 25, 3, 458, DateTimeKind.Utc).AddTicks(5317));
        }
    }
}
