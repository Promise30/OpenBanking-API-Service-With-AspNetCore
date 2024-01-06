using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenBanking_API_Service.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAccountRelatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDeposits_BankAccounts_BankAccountId",
                table: "BankDeposits");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4100fb4-2f04-453e-9b93-e5250630b706");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e101a69a-db2c-4243-b560-48d5a1416e5b");

            migrationBuilder.RenameColumn(
                name: "BankAccountId",
                table: "BankDeposits",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankDeposits_BankAccountId",
                table: "BankDeposits",
                newName: "IX_BankDeposits_AccountId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1960b464-2e64-4179-96d2-71beaa15fc09", "1", "Admin", "ADMIN" },
                    { "e3123a46-4f72-4d63-a3f6-2f77ba9e48ba", "2", "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankDeposits_BankAccounts_AccountId",
                table: "BankDeposits",
                column: "AccountId",
                principalTable: "BankAccounts",
                principalColumn: "BankAccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDeposits_BankAccounts_AccountId",
                table: "BankDeposits");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1960b464-2e64-4179-96d2-71beaa15fc09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3123a46-4f72-4d63-a3f6-2f77ba9e48ba");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "BankDeposits",
                newName: "BankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankDeposits_AccountId",
                table: "BankDeposits",
                newName: "IX_BankDeposits_BankAccountId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d4100fb4-2f04-453e-9b93-e5250630b706", "2", "User", "USER" },
                    { "e101a69a-db2c-4243-b560-48d5a1416e5b", "1", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankDeposits_BankAccounts_BankAccountId",
                table: "BankDeposits",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "BankAccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
