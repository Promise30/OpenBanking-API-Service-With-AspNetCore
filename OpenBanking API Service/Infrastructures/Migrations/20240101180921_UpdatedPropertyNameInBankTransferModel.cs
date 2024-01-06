using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenBanking_API_Service.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPropertyNameInBankTransferModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransfers_BankAccounts_BankAccountId",
                table: "BankTransfers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "188bc835-30b9-42de-813e-54910c9d26ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29d3dbad-3c79-4cd7-9b91-6c0bcc1463d4");

            migrationBuilder.RenameColumn(
                name: "BankAccountId",
                table: "BankTransfers",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankTransfers_BankAccountId",
                table: "BankTransfers",
                newName: "IX_BankTransfers_AccountId");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "BankTransfers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d4100fb4-2f04-453e-9b93-e5250630b706", "2", "User", "USER" },
                    { "e101a69a-db2c-4243-b560-48d5a1416e5b", "1", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransfers_BankAccounts_AccountId",
                table: "BankTransfers",
                column: "AccountId",
                principalTable: "BankAccounts",
                principalColumn: "BankAccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransfers_BankAccounts_AccountId",
                table: "BankTransfers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4100fb4-2f04-453e-9b93-e5250630b706");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e101a69a-db2c-4243-b560-48d5a1416e5b");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "BankTransfers");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "BankTransfers",
                newName: "BankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankTransfers_AccountId",
                table: "BankTransfers",
                newName: "IX_BankTransfers_BankAccountId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "188bc835-30b9-42de-813e-54910c9d26ed", "1", "Admin", "ADMIN" },
                    { "29d3dbad-3c79-4cd7-9b91-6c0bcc1463d4", "2", "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransfers_BankAccounts_BankAccountId",
                table: "BankTransfers",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "BankAccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
