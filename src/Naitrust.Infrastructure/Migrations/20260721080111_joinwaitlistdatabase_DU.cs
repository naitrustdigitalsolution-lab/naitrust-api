using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitrust.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class joinwaitlistdatabase_DU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "WaitlistEntries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Consent",
                table: "WaitlistEntries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Expectations",
                table: "WaitlistEntries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "WaitlistEntries",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionNeed",
                table: "WaitlistEntries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionRange",
                table: "WaitlistEntries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "WaitlistEntries",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "Consent",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "Expectations",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "TransactionNeed",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "TransactionRange",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "WaitlistEntries");
        }
    }
}
