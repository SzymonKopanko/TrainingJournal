using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingJournalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToExerciseSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExerciseSets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSets_UserId",
                table: "ExerciseSets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseSets_AspNetUsers_UserId",
                table: "ExerciseSets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseSets_AspNetUsers_UserId",
                table: "ExerciseSets");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseSets_UserId",
                table: "ExerciseSets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExerciseSets");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "2eb306f0-9884-43f8-a551-8df42df45011", new DateTime(2025, 6, 19, 12, 11, 5, 342, DateTimeKind.Utc).AddTicks(4380), "AQAAAAIAAYagAAAAEFNpDod2hDhig8mAyoux5XV9NwWxt/6X17VqQ89PiKLFWORyOFa0afOaiBqdL8maKw==" });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 12, 11, 5, 395, DateTimeKind.Utc).AddTicks(5523));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 12, 11, 5, 395, DateTimeKind.Utc).AddTicks(5536));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 19, 12, 11, 5, 395, DateTimeKind.Utc).AddTicks(5538));

            migrationBuilder.UpdateData(
                table: "UserWeights",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "WeightedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 12, 11, 5, 395, DateTimeKind.Utc).AddTicks(5638), new DateTime(2025, 6, 19, 12, 11, 5, 395, DateTimeKind.Utc).AddTicks(5637) });
        }
    }
}
