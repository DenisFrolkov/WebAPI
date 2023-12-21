using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19de1a3d-e9da-41a4-8876-4ff555615a1f", "01da50a9-f7b4-4ba0-914f-74c9bd481eaa", "Administrator", "ADMINISTRATOR" },
                    { "27157166-361b-49ca-b213-516e5e973914", "035436dd-1641-4dff-bfcf-061ca28f8e7f", "Manager", "MANAGER" }
                });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: new Guid("0c9de349-d07a-4752-8e32-90fdfa01ad1c"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 1, 20, 11, 59, 59, 271, DateTimeKind.Local).AddTicks(230), new DateTime(2023, 12, 21, 11, 59, 59, 271, DateTimeKind.Local).AddTicks(217) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: new Guid("b5ba1486-f2d4-48b5-b78e-4cc807bcaffd"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 2, 4, 11, 59, 59, 271, DateTimeKind.Local).AddTicks(245), new DateTime(2023, 12, 21, 11, 59, 59, 271, DateTimeKind.Local).AddTicks(245) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19de1a3d-e9da-41a4-8876-4ff555615a1f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27157166-361b-49ca-b213-516e5e973914");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: new Guid("0c9de349-d07a-4752-8e32-90fdfa01ad1c"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 1, 20, 11, 56, 1, 439, DateTimeKind.Local).AddTicks(3900), new DateTime(2023, 12, 21, 11, 56, 1, 439, DateTimeKind.Local).AddTicks(3886) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: new Guid("b5ba1486-f2d4-48b5-b78e-4cc807bcaffd"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 2, 4, 11, 56, 1, 439, DateTimeKind.Local).AddTicks(3911), new DateTime(2023, 12, 21, 11, 56, 1, 439, DateTimeKind.Local).AddTicks(3910) });
        }
    }
}
