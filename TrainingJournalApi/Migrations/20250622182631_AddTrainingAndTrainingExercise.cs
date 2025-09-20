using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingJournalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingAndTrainingExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingExercises_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_TrainingExercises_ExerciseId",
                table: "TrainingExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingExercises_TrainingId",
                table: "TrainingExercises",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_UserId",
                table: "Trainings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingExercises");

            migrationBuilder.DropTable(
                name: "Trainings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "01415176-ed4e-44a9-92e0-9adbe7d6fcfa", new DateTime(2025, 6, 19, 12, 17, 10, 668, DateTimeKind.Utc).AddTicks(5469), "AQAAAAIAAYagAAAAEIos2DLWp0DyI2pEetShmxAuloaNdXEj3G3HbNEmATbtuZleceBad5rLV2qYcYp2Eg==" });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 12, 17, 10, 725, DateTimeKind.Utc).AddTicks(8110));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 12, 17, 10, 725, DateTimeKind.Utc).AddTicks(8112));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 12, 17, 10, 725, DateTimeKind.Utc).AddTicks(8120));

            migrationBuilder.UpdateData(
                table: "UserWeights",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "WeightedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 12, 17, 10, 725, DateTimeKind.Utc).AddTicks(8174), new DateTime(2025, 6, 19, 12, 17, 10, 725, DateTimeKind.Utc).AddTicks(8173) });
        }
    }
}
