using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Taxes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    City = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Taxes",
                columns: new[] { "Id", "Category", "City", "CreatedAt", "ModifiedAt", "Rate", "StartDate" },
                values: new object[,]
                {
                    { new Guid("0b9429e3-bc68-48c7-b69e-cdc49bc2823f"), 2, "Kaunas", new DateTime(2024, 2, 18, 20, 35, 32, 880, DateTimeKind.Utc).AddTicks(4130), null, 4m, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("0fc45dce-14b9-4d76-bad0-92f8540ae109"), 4, "Kaunas", new DateTime(2024, 2, 18, 20, 35, 32, 880, DateTimeKind.Utc).AddTicks(4150), null, 1.2m, new DateTime(2024, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("2362d6ae-f2f9-4eb2-ab3c-5d5e9cabc185"), 3, "Kaunas", new DateTime(2024, 2, 18, 20, 35, 32, 880, DateTimeKind.Utc).AddTicks(4130), null, 2.5m, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("2b103a0d-2eb7-4094-b455-37f23892c7a8"), 2, "Kaunas", new DateTime(2024, 2, 18, 20, 35, 32, 880, DateTimeKind.Utc).AddTicks(4130), null, 6m, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("69c96c48-4ba0-435b-842c-1b55d35aa79c"), 3, "Kaunas", new DateTime(2024, 2, 18, 20, 35, 32, 880, DateTimeKind.Utc).AddTicks(4140), null, 2.5m, new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("79dd5c83-166f-496c-b5c7-0d9932c387c8"), 4, "Kaunas", new DateTime(2024, 2, 18, 20, 35, 32, 880, DateTimeKind.Utc).AddTicks(4140), null, 1.5m, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e0b8e34e-dc33-4d3b-b73e-ed899bf584f8"), 2, "Kaunas", new DateTime(2024, 2, 18, 20, 35, 32, 880, DateTimeKind.Utc).AddTicks(4120), null, 5m, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("fea10830-70ad-4ae6-9ed8-b9f53763cb3a"), 1, "Kaunas", new DateTime(2024, 2, 18, 20, 35, 32, 880, DateTimeKind.Utc).AddTicks(4110), null, 3.3m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "Index_City",
                table: "Taxes",
                column: "City");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Taxes");
        }
    }
}
