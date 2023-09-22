using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "EmployeeId", "Manager", "Name" },
                values: new object[,]
                {
                    { new Guid("2a30ca60-117b-4b2d-a52d-5d912d7d2676"), new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), "Jane Smith", "HR Department" },
                    { new Guid("9ce18e7b-82b3-4be1-bd02-bdc34cdbf10e"), new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), "John Doe", "IT Department" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectId", "CompanyId", "Description", "EndDate", "Name", "StartDate" },
                values: new object[,]
                {
                    { new Guid("0c9de349-d07a-4752-8e32-90fdfa01ad1c"), new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Description for Project A", new DateTime(2023, 10, 22, 10, 26, 47, 825, DateTimeKind.Local).AddTicks(3987), "Project A", new DateTime(2023, 9, 22, 10, 26, 47, 825, DateTimeKind.Local).AddTicks(3973) },
                    { new Guid("b5ba1486-f2d4-48b5-b78e-4cc807bcaffd"), new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Description for Project B", new DateTime(2023, 11, 6, 10, 26, 47, 825, DateTimeKind.Local).AddTicks(3998), "Project B", new DateTime(2023, 9, 22, 10, 26, 47, 825, DateTimeKind.Local).AddTicks(3997) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("2a30ca60-117b-4b2d-a52d-5d912d7d2676"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("9ce18e7b-82b3-4be1-bd02-bdc34cdbf10e"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: new Guid("0c9de349-d07a-4752-8e32-90fdfa01ad1c"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectId",
                keyValue: new Guid("b5ba1486-f2d4-48b5-b78e-4cc807bcaffd"));
        }
    }
}
