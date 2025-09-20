using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingJournalApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCalculatedColumnsFromExerciseSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OneRepMax",
                table: "ExerciseSets");

            migrationBuilder.DropColumn(
                name: "PercievedOneRepMax",
                table: "ExerciseSets");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "0257c9b6-2e9a-4cb0-aca4-8149e5780ed4", new DateTime(2025, 8, 23, 7, 35, 31, 835, DateTimeKind.Utc).AddTicks(2663), "AQAAAAIAAYagAAAAEPNKjb3Ch7+r7lrkhVOsvaTho7iR+MNFZxJPfUFSN3C4KOgDIySYYOxn59BS69wl1g==" });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 23, 7, 35, 31, 885, DateTimeKind.Utc).AddTicks(9139));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 23, 7, 35, 31, 885, DateTimeKind.Utc).AddTicks(9142));

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 23, 7, 35, 31, 885, DateTimeKind.Utc).AddTicks(9144));

            migrationBuilder.UpdateData(
                table: "UserWeights",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "WeightedAt" },
                values: new object[] { new DateTime(2025, 8, 23, 7, 35, 31, 885, DateTimeKind.Utc).AddTicks(9178), new DateTime(2025, 8, 23, 7, 35, 31, 885, DateTimeKind.Utc).AddTicks(9177) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OneRepMax",
                table: "ExerciseSets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercievedOneRepMax",
                table: "ExerciseSets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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
        }
    }
}
