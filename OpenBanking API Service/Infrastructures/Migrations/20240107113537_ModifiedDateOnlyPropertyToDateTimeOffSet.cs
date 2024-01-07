using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenBanking_API_Service.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedDateOnlyPropertyToDateTimeOffSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "609baaee-d054-4274-a234-1403db63a407");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6f0f6ac-d98e-493a-8566-ceb51c438174");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateOfBirth",
                table: "BankAccounts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "428c01f2-c71f-4699-aca1-bc7fc6d3c765", "2", "User", "USER" },
                    { "9dba8f44-a4fa-4bcc-a953-6a01a1ab1f6c", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "428c01f2-c71f-4699-aca1-bc7fc6d3c765");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9dba8f44-a4fa-4bcc-a953-6a01a1ab1f6c");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "BankAccounts",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "609baaee-d054-4274-a234-1403db63a407", "2", "User", "USER" },
                    { "b6f0f6ac-d98e-493a-8566-ceb51c438174", "1", "Admin", "ADMIN" }
                });
        }
    }
}
