using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingJournalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseEntries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseEntries_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseEntries_ExerciseId",
                table: "ExerciseEntries",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseEntries_UserId",
                table: "ExerciseEntries",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseEntries");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "24ed80b2-329d-48cb-96f3-3886c727c9bc", new DateTime(2025, 6, 19, 10, 43, 13, 643, DateTimeKind.Utc).AddTicks(8672), "AQAAAAIAAYagAAAAEEvldUjPu1SBDOJ/dIfOIs2E+q6NBY6RaXShdW1aqETmb03iRRvFlCkTphV0SDSz6Q==" });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 10, 43, 13, 700, DateTimeKind.Utc).AddTicks(9726));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 10, 43, 13, 700, DateTimeKind.Utc).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 10, 43, 13, 700, DateTimeKind.Utc).AddTicks(9730));
        }
    }
}
