using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenBanking_API_Service.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedBankAccountDateOfBirthPropertyToDateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1960b464-2e64-4179-96d2-71beaa15fc09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3123a46-4f72-4d63-a3f6-2f77ba9e48ba");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "BankAccounts",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "609baaee-d054-4274-a234-1403db63a407", "2", "User", "USER" },
                    { "b6f0f6ac-d98e-493a-8566-ceb51c438174", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "609baaee-d054-4274-a234-1403db63a407");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6f0f6ac-d98e-493a-8566-ceb51c438174");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "BankAccounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1960b464-2e64-4179-96d2-71beaa15fc09", "1", "Admin", "ADMIN" },
                    { "e3123a46-4f72-4d63-a3f6-2f77ba9e48ba", "2", "User", "USER" }
                });
        }
    }
}
