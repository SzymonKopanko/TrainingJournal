using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingJournalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserWeights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    WeightedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWeights_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "UserWeights",
                columns: new[] { "Id", "CreatedAt", "UpdatedAt", "UserId", "Weight", "WeightedAt" },
                values: new object[] { 1, new DateTime(2025, 6, 19, 12, 11, 5, 395, DateTimeKind.Utc).AddTicks(5638), null, "seed-user-id", 80.0, new DateTime(2025, 6, 19, 12, 11, 5, 395, DateTimeKind.Utc).AddTicks(5637) });

            migrationBuilder.CreateIndex(
                name: "IX_UserWeights_UserId",
                table: "UserWeights",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWeights");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "Weight" },
                values: new object[] { "d21116a6-f3fb-425d-8c9e-66143eedf3f1", new DateTime(2025, 6, 19, 11, 34, 33, 96, DateTimeKind.Utc).AddTicks(9118), "AQAAAAIAAYagAAAAEKqsMXrUXmXuN2NvaYlcenSldf8cApc0m2Pmqm8rV3nRYNw2iBWfYf60CGvPfKPfqA==", 80.0 });

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
        }
    }
}
