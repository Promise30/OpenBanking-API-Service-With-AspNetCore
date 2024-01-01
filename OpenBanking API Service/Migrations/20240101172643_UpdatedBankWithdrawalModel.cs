using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenBanking_API_Service.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBankWithdrawalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb6c801e-cfd1-481d-bdd0-912c9a97244f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb96eb18-646c-47d3-98c1-be8d126a35ba");

            migrationBuilder.DropColumn(
                name: "Pin",
                table: "BankWithdrawals");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "BankWithdrawals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "BankWithdrawals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TransactionDate",
                table: "BankWithdrawals",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "BankDeposits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "BankDeposits",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f7955d9-ff5e-4178-8fcc-e6e86d144b69", "1", "Admin", "ADMIN" },
                    { "ed38f383-5ef0-4622-a4d2-493d016040a9", "2", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f7955d9-ff5e-4178-8fcc-e6e86d144b69");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed38f383-5ef0-4622-a4d2-493d016040a9");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "BankWithdrawals");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "BankWithdrawals");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "BankWithdrawals");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "BankDeposits");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "BankDeposits");

            migrationBuilder.AddColumn<int>(
                name: "Pin",
                table: "BankWithdrawals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cb6c801e-cfd1-481d-bdd0-912c9a97244f", "2", "User", "USER" },
                    { "cb96eb18-646c-47d3-98c1-be8d126a35ba", "1", "Admin", "ADMIN" }
                });
        }
    }
}
