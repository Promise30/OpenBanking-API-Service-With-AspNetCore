using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenBanking_API_Service.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBankTransferModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Pin",
                table: "BankTransfers");

            migrationBuilder.AlterColumn<string>(
                name: "DestinationAccount",
                table: "BankTransfers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "SourceAccount",
                table: "BankTransfers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "188bc835-30b9-42de-813e-54910c9d26ed", "1", "Admin", "ADMIN" },
                    { "29d3dbad-3c79-4cd7-9b91-6c0bcc1463d4", "2", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "188bc835-30b9-42de-813e-54910c9d26ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29d3dbad-3c79-4cd7-9b91-6c0bcc1463d4");

            migrationBuilder.DropColumn(
                name: "SourceAccount",
                table: "BankTransfers");

            migrationBuilder.AlterColumn<Guid>(
                name: "DestinationAccount",
                table: "BankTransfers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Pin",
                table: "BankTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f7955d9-ff5e-4178-8fcc-e6e86d144b69", "1", "Admin", "ADMIN" },
                    { "ed38f383-5ef0-4622-a4d2-493d016040a9", "2", "User", "USER" }
                });
        }
    }
}
