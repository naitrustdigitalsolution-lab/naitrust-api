using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitrust.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Deal_StagedPayments_DisputeEvidence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivePaymentStage",
                table: "Transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPaymentReleasedAt",
                table: "Transactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InitialPaymentMinor",
                table: "Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextPaymentReleaseConditions",
                table: "Transactions",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RemainingPaymentMinor",
                table: "Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AllocationStage1Minor",
                table: "TransactionParties",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AllocationStage2Minor",
                table: "TransactionParties",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasEvidence",
                table: "Disputes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivePaymentStage",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FirstPaymentReleasedAt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "InitialPaymentMinor",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "NextPaymentReleaseConditions",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RemainingPaymentMinor",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AllocationStage1Minor",
                table: "TransactionParties");

            migrationBuilder.DropColumn(
                name: "AllocationStage2Minor",
                table: "TransactionParties");

            migrationBuilder.DropColumn(
                name: "HasEvidence",
                table: "Disputes");
        }
    }
}
