using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenBanking_API_Service.Migrations
{
    /// <inheritdoc />
    public partial class AddedAccountsRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7955caff-f101-48c3-9f83-faebd1db3736");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc042817-efa4-4277-aafd-e50d71200214");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    BankAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountBalance = table.Column<double>(type: "float", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Middlename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pin = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountOpeningDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.BankAccountId);
                    table.ForeignKey(
                        name: "FK_BankAccounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankDeposits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Pin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDeposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDeposits_BankAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "BankAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Pin = table.Column<int>(type: "int", nullable: false),
                    BankAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransfers_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "BankAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankWithdrawals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Pin = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankWithdrawals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankWithdrawals_BankAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "BankAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15f939af-838e-4267-966f-f5558c3a0366", "2", "User", "USER" },
                    { "a367dcdd-58d4-4ffa-9bc9-408be82548cd", "1", "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankDeposits_AccountId",
                table: "BankDeposits",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransfers_BankAccountId",
                table: "BankTransfers",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankWithdrawals_AccountId",
                table: "BankWithdrawals",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankDeposits");

            migrationBuilder.DropTable(
                name: "BankTransfers");

            migrationBuilder.DropTable(
                name: "BankWithdrawals");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15f939af-838e-4267-966f-f5558c3a0366");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a367dcdd-58d4-4ffa-9bc9-408be82548cd");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7955caff-f101-48c3-9f83-faebd1db3736", "2", "User", "USER" },
                    { "cc042817-efa4-4277-aafd-e50d71200214", "1", "Admin", "ADMIN" }
                });
        }
    }
}
