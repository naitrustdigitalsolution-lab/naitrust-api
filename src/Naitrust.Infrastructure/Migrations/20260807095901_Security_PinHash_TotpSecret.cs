using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitrust.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Security_PinHash_TotpSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KycLevel",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinHash",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotpSecret",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionTypeId",
                table: "Transactions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "DealType",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryDueDate",
                table: "Transactions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "Transactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtendedProductTestingDays",
                table: "Transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousReference",
                table: "Transactions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Recurring",
                table: "Transactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReleaseConditions",
                table: "Transactions",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UseCase",
                table: "Transactions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AllocationMinor",
                table: "TransactionParties",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Milestones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedAt",
                table: "Milestones",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByUserId",
                table: "Milestones",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Businesses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Businesses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Businesses",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Businesses",
                type: "character varying(320)",
                maxLength: 320,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NtId",
                table: "Businesses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "Businesses",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentAccountBankName",
                table: "Businesses",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentAccountName",
                table: "Businesses",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentAccountNumber",
                table: "Businesses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Businesses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Businesses",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialHandles",
                table: "Businesses",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Businesses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GeneratedByAi",
                table: "Agreements",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SectionsJson",
                table: "Agreements",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DealInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PublicToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RecipientUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IntendedContact = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    IntendedAccountType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    InviterProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    InviteeProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    PostAuthDestination = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FromName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FromRole = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    YourRole = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PartyMode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    AmountMinor = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AgreementSnapshot = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealInvitations_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealInvitations_Users_RecipientUserId",
                        column: x => x.RecipientUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DealMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealTerminations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RespondedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    RespondedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResponseReason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealTerminations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NegotiationProposals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NegotiationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProposedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProposedChangesJson = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NegotiationProposals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Negotiations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    InitiatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LatestProposalId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negotiations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_NtId",
                table: "Businesses",
                column: "NtId",
                unique: true,
                filter: "\"NtId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_Slug",
                table: "Businesses",
                column: "Slug",
                unique: true,
                filter: "\"Slug\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DealInvitations_PublicToken",
                table: "DealInvitations",
                column: "PublicToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealInvitations_RecipientUserId",
                table: "DealInvitations",
                column: "RecipientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealInvitations_Status",
                table: "DealInvitations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DealInvitations_TransactionId",
                table: "DealInvitations",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DealMessages_TransactionId",
                table: "DealMessages",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DealTerminations_TransactionId",
                table: "DealTerminations",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_NegotiationProposals_NegotiationId",
                table: "NegotiationProposals",
                column: "NegotiationId");

            migrationBuilder.CreateIndex(
                name: "IX_Negotiations_TransactionId",
                table: "Negotiations",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealInvitations");

            migrationBuilder.DropTable(
                name: "DealMessages");

            migrationBuilder.DropTable(
                name: "DealTerminations");

            migrationBuilder.DropTable(
                name: "NegotiationProposals");

            migrationBuilder.DropTable(
                name: "Negotiations");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_NtId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_Slug",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "KycLevel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PinHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotpSecret",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DealType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DeliveryDueDate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ExtendedProductTestingDays",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PreviousReference",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Recurring",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReleaseConditions",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UseCase",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AllocationMinor",
                table: "TransactionParties");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "StatusChangedAt",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "NtId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PaymentAccountBankName",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PaymentAccountName",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "PaymentAccountNumber",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "SocialHandles",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "GeneratedByAi",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "SectionsJson",
                table: "Agreements");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionTypeId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Businesses",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
