using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenBanking_API_Service.Migrations
{
    /// <inheritdoc />
    public partial class NewPropertiesToBankAccountModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "428c01f2-c71f-4699-aca1-bc7fc6d3c765");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9dba8f44-a4fa-4bcc-a953-6a01a1ab1f6c");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "BankWithdrawals",
                newName: "AccountBalance");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "BankTransfers",
                newName: "AccountBalance");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "BankDeposits",
                newName: "AccountBalance");

            migrationBuilder.RenameColumn(
                name: "Middlename",
                table: "BankAccounts",
                newName: "MiddleName");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "BankAccounts",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "BankAccounts",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "StateOfOrigin",
                table: "BankAccounts",
                newName: "ResidentCountry");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "BankAccounts",
                newName: "ResidentAddress");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AspNetUsers",
                newName: "ModifiedDate");

            migrationBuilder.AlterColumn<string>(
                name: "Narration",
                table: "BankTransfers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "BankAccounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<string>(
                name: "BirthCountry",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResidentPostalCode",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2572df33-2465-4e47-aab7-32c23967493b", "2", "User", "USER" },
                    { "f0024021-7807-43c4-a476-4a94e896a252", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2572df33-2465-4e47-aab7-32c23967493b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0024021-7807-43c4-a476-4a94e896a252");

            migrationBuilder.DropColumn(
                name: "BirthCountry",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "City",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "ResidentPostalCode",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "State",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AccountBalance",
                table: "BankWithdrawals",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "AccountBalance",
                table: "BankTransfers",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "AccountBalance",
                table: "BankDeposits",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "BankAccounts",
                newName: "Middlename");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "BankAccounts",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "BankAccounts",
                newName: "Firstname");

            migrationBuilder.RenameColumn(
                name: "ResidentCountry",
                table: "BankAccounts",
                newName: "StateOfOrigin");

            migrationBuilder.RenameColumn(
                name: "ResidentAddress",
                table: "BankAccounts",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "AspNetUsers",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Narration",
                table: "BankTransfers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Middlename",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateOfBirth",
                table: "BankAccounts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "428c01f2-c71f-4699-aca1-bc7fc6d3c765", "2", "User", "USER" },
                    { "9dba8f44-a4fa-4bcc-a953-6a01a1ab1f6c", "1", "Admin", "ADMIN" }
                });
        }
    }
}
