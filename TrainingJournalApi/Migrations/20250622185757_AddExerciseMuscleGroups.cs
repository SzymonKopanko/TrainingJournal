using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingJournalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseMuscleGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseMuscleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleGroup = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMuscleGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleGroups_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "20cf15b1-e3a2-440c-b583-7dd301dea9b5", new DateTime(2025, 6, 22, 18, 57, 57, 119, DateTimeKind.Utc).AddTicks(7247), "AQAAAAIAAYagAAAAEJ/G/+TGqFjK2q0kv5LsM+xLqcpTJOCPYi9Oopq+hi99JjpwMFPPhuU6l32lGHav4g==" });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 18, 57, 57, 174, DateTimeKind.Utc).AddTicks(1822));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 18, 57, 57, 174, DateTimeKind.Utc).AddTicks(1824));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 18, 57, 57, 174, DateTimeKind.Utc).AddTicks(1826));

            migrationBuilder.UpdateData(
                table: "UserWeights",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "WeightedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 18, 57, 57, 174, DateTimeKind.Utc).AddTicks(1964), new DateTime(2025, 6, 22, 18, 57, 57, 174, DateTimeKind.Utc).AddTicks(1963) });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscleGroups_ExerciseId",
                table: "ExerciseMuscleGroups",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseMuscleGroups");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "a5136c52-5455-497e-9584-4b4babf4d8ea", new DateTime(2025, 6, 22, 18, 26, 30, 941, DateTimeKind.Utc).AddTicks(9162), "AQAAAAIAAYagAAAAEJ8BvKZemqTO5tvS3qwYkei14Y6Dc57HxPoebKZ8z99Vd7iqihkzOIyetdiKECccEw==" });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 18, 26, 30, 997, DateTimeKind.Utc).AddTicks(9664));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 18, 26, 30, 997, DateTimeKind.Utc).AddTicks(9669));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 18, 26, 30, 997, DateTimeKind.Utc).AddTicks(9671));

            migrationBuilder.UpdateData(
                table: "UserWeights",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "WeightedAt" },
                values: new object[] { new DateTime(2025, 6, 22, 18, 26, 30, 997, DateTimeKind.Utc).AddTicks(9732), new DateTime(2025, 6, 22, 18, 26, 30, 997, DateTimeKind.Utc).AddTicks(9731) });
        }
    }
}
