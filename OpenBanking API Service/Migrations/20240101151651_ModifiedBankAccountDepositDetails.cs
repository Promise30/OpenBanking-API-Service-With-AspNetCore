using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenBanking_API_Service.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedBankAccountDepositDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDeposits_BankAccounts_AccountId",
                table: "BankDeposits");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15f939af-838e-4267-966f-f5558c3a0366");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a367dcdd-58d4-4ffa-9bc9-408be82548cd");

            migrationBuilder.DropColumn(
                name: "Pin",
                table: "BankDeposits");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "BankDeposits",
                newName: "BankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankDeposits_AccountId",
                table: "BankDeposits",
                newName: "IX_BankDeposits_BankAccountId");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TransactionDate",
                table: "BankDeposits",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cb6c801e-cfd1-481d-bdd0-912c9a97244f", "2", "User", "USER" },
                    { "cb96eb18-646c-47d3-98c1-be8d126a35ba", "1", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankDeposits_BankAccounts_BankAccountId",
                table: "BankDeposits",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "BankAccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDeposits_BankAccounts_BankAccountId",
                table: "BankDeposits");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb6c801e-cfd1-481d-bdd0-912c9a97244f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb96eb18-646c-47d3-98c1-be8d126a35ba");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "BankDeposits");

            migrationBuilder.RenameColumn(
                name: "BankAccountId",
                table: "BankDeposits",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankDeposits_BankAccountId",
                table: "BankDeposits",
                newName: "IX_BankDeposits_AccountId");

            migrationBuilder.AddColumn<int>(
                name: "Pin",
                table: "BankDeposits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15f939af-838e-4267-966f-f5558c3a0366", "2", "User", "USER" },
                    { "a367dcdd-58d4-4ffa-9bc9-408be82548cd", "1", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankDeposits_BankAccounts_AccountId",
                table: "BankDeposits",
                column: "AccountId",
                principalTable: "BankAccounts",
                principalColumn: "BankAccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
