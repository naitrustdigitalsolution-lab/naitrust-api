using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Naitrust.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class databasedesign_DU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VirtualAccount",
                table: "VirtualAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationStep",
                table: "VerificationStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationRequest",
                table: "VerificationRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationProviderEvent",
                table: "VerificationProviderEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationDocument",
                table: "VerificationDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VectorDocument",
                table: "VectorDocument");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionType",
                table: "TransactionType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionParty",
                table: "TransactionParty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReputationProfile",
                table: "ReputationProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReleaseRequest",
                table: "ReleaseRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayoutAccount",
                table: "PayoutAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentPartnerEvent",
                table: "PaymentPartnerEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentInstruction",
                table: "PaymentInstruction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Party",
                table: "Party");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnershipCheck",
                table: "OwnershipCheck");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Milestone",
                table: "Milestone");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LedgerEntry",
                table: "LedgerEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FaceMatchResult",
                table: "FaceMatchResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvidenceFile",
                table: "EvidenceFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DisputeMessage",
                table: "DisputeMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dispute",
                table: "Dispute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessMember",
                table: "BusinessMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Business",
                table: "Business");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AiPromptVersion",
                table: "AiPromptVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AiFeedback",
                table: "AiFeedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AiAssessment",
                table: "AiAssessment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agreement",
                table: "Agreement");

            migrationBuilder.DropColumn(
                name: "AmountExpectedMinor",
                table: "VirtualAccount");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "VirtualAccount");

            migrationBuilder.RenameTable(
                name: "VirtualAccount",
                newName: "VirtualAccounts");

            migrationBuilder.RenameTable(
                name: "VerificationStep",
                newName: "VerificationSteps");

            migrationBuilder.RenameTable(
                name: "VerificationRequest",
                newName: "VerificationRequests");

            migrationBuilder.RenameTable(
                name: "VerificationProviderEvent",
                newName: "VerificationProviderEvents");

            migrationBuilder.RenameTable(
                name: "VerificationDocument",
                newName: "VerificationDocuments");

            migrationBuilder.RenameTable(
                name: "VectorDocument",
                newName: "VectorDocuments");

            migrationBuilder.RenameTable(
                name: "TransactionType",
                newName: "TransactionTypes");

            migrationBuilder.RenameTable(
                name: "TransactionParty",
                newName: "TransactionParties");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "ReputationProfile",
                newName: "ReputationProfiles");

            migrationBuilder.RenameTable(
                name: "ReleaseRequest",
                newName: "ReleaseRequests");

            migrationBuilder.RenameTable(
                name: "PayoutAccount",
                newName: "PayoutAccounts");

            migrationBuilder.RenameTable(
                name: "PaymentPartnerEvent",
                newName: "PaymentPartnerEvents");

            migrationBuilder.RenameTable(
                name: "PaymentInstruction",
                newName: "PaymentInstructions");

            migrationBuilder.RenameTable(
                name: "Party",
                newName: "Parties");

            migrationBuilder.RenameTable(
                name: "OwnershipCheck",
                newName: "OwnershipChecks");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Milestone",
                newName: "Milestones");

            migrationBuilder.RenameTable(
                name: "LedgerEntry",
                newName: "LedgerEntries");

            migrationBuilder.RenameTable(
                name: "FaceMatchResult",
                newName: "FaceMatchResults");

            migrationBuilder.RenameTable(
                name: "EvidenceFile",
                newName: "EvidenceFiles");

            migrationBuilder.RenameTable(
                name: "DisputeMessage",
                newName: "DisputeMessages");

            migrationBuilder.RenameTable(
                name: "Dispute",
                newName: "Disputes");

            migrationBuilder.RenameTable(
                name: "BusinessMember",
                newName: "BusinessMembers");

            migrationBuilder.RenameTable(
                name: "Business",
                newName: "Businesses");

            migrationBuilder.RenameTable(
                name: "AiPromptVersion",
                newName: "AiPromptVersions");

            migrationBuilder.RenameTable(
                name: "AiFeedback",
                newName: "AiFeedbacks");

            migrationBuilder.RenameTable(
                name: "AiAssessment",
                newName: "AiAssessments");

            migrationBuilder.RenameTable(
                name: "Agreement",
                newName: "Agreements");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "OutboxMessages",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "IdempotencyKeys",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestHash",
                table: "IdempotencyKeys",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "IdempotencyKeys",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "VirtualAccounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "VirtualAccounts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Partner",
                table: "VirtualAccounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "VirtualAccounts",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "VirtualAccounts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "VirtualAccounts",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "VirtualAccounts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "VirtualAccounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "VirtualAccounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "VirtualAccounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "VirtualAccounts",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AlterColumn<string>(
                name: "Step",
                table: "VerificationSteps",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "VerificationSteps",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "VerificationSteps",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VerificationType",
                table: "VerificationRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "VerificationLevel",
                table: "VerificationRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectType",
                table: "VerificationRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "VerificationRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "VerificationRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "VerificationRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "VerificationRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentReference",
                table: "VerificationRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "VerificationProviderEvents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "VerificationProviderEvents",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "VerificationProviderEvents",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "VerificationDocuments",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "VerificationDocuments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "VerificationDocuments",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "VerificationDocuments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentType",
                table: "VerificationDocuments",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "SourceType",
                table: "VectorDocuments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EmbeddingModel",
                table: "VectorDocuments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RequiredVerificationLevel",
                table: "TransactionTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ReleaseMode",
                table: "TransactionTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransactionTypes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "TransactionTypes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TransactionParties",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "TransactionParties",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartyType",
                table: "TransactionParties",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "PartyMode",
                table: "TransactionParties",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "TransactionParties",
                type: "character varying(320)",
                maxLength: 320,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "TransactionParties",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "VerificationLevelRequired",
                table: "Transactions",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Transactions",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Transactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "RiskLevel",
                table: "Transactions",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Transactions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Transactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "PartyMode",
                table: "Transactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Transactions",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Transactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Transactions",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AlterColumn<string>(
                name: "RevieweeSubjectType",
                table: "Reviews",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectType",
                table: "ReputationProfiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "RatingAverage",
                table: "ReputationProfiles",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ReleaseRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "ReleaseRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "ReleaseRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "ReleaseRequests",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "PayoutAccounts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameMatchStatus",
                table: "PayoutAccounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "PayoutAccounts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankCode",
                table: "PayoutAccounts",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumberHash",
                table: "PayoutAccounts",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "PayoutAccounts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderEventId",
                table: "PaymentPartnerEvents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Partner",
                table: "PaymentPartnerEvents",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "PaymentPartnerEvents",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PaymentInstructions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "SignedPayloadHash",
                table: "PaymentInstructions",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartnerReference",
                table: "PaymentInstructions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Partner",
                table: "PaymentInstructions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "InstructionType",
                table: "PaymentInstructions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "IdempotencyKey",
                table: "PaymentInstructions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "VerificationStatus",
                table: "Parties",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartyType",
                table: "Parties",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Parties",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CacReference",
                table: "Parties",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BvnReference",
                table: "Parties",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "OwnershipChecks",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Method",
                table: "OwnershipChecks",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Milestones",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Milestones",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "LedgerEntries",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "LedgerEntries",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Account",
                table: "LedgerEntries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LedgerEntries",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "FaceMatchResults",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "MatchScore",
                table: "FaceMatchResults",
                type: "numeric(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "IdType",
                table: "FaceMatchResults",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "IdNumberHash",
                table: "FaceMatchResults",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Confidence",
                table: "FaceMatchResults",
                type: "numeric(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "EvidenceFiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "EvidenceFiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "EvidenceFiles",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "EvidenceFiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Disputes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Resolution",
                table: "Disputes",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Disputes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "BusinessMembers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "BusinessMembers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "VerificationStatus",
                table: "Businesses",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Businesses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TaxId",
                table: "Businesses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Businesses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RiskLevel",
                table: "Businesses",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Businesses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Businesses",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Businesses",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Businesses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "AiPromptVersions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AiPromptVersions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FeedbackType",
                table: "AiFeedbacks",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "RiskLevel",
                table: "AiAssessments",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "AiAssessments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EntityType",
                table: "AiAssessments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "AiAssessments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Confidence",
                table: "AiAssessments",
                type: "numeric(5,4)",
                precision: 5,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssessmentType",
                table: "AiAssessments",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VirtualAccounts",
                table: "VirtualAccounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationSteps",
                table: "VerificationSteps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationRequests",
                table: "VerificationRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationProviderEvents",
                table: "VerificationProviderEvents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationDocuments",
                table: "VerificationDocuments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VectorDocuments",
                table: "VectorDocuments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionTypes",
                table: "TransactionTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionParties",
                table: "TransactionParties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReputationProfiles",
                table: "ReputationProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReleaseRequests",
                table: "ReleaseRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayoutAccounts",
                table: "PayoutAccounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentPartnerEvents",
                table: "PaymentPartnerEvents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentInstructions",
                table: "PaymentInstructions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parties",
                table: "Parties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnershipChecks",
                table: "OwnershipChecks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Milestones",
                table: "Milestones",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LedgerEntries",
                table: "LedgerEntries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FaceMatchResults",
                table: "FaceMatchResults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvidenceFiles",
                table: "EvidenceFiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisputeMessages",
                table: "DisputeMessages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disputes",
                table: "Disputes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessMembers",
                table: "BusinessMembers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Businesses",
                table: "Businesses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AiPromptVersions",
                table: "AiPromptVersions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AiFeedbacks",
                table: "AiFeedbacks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AiAssessments",
                table: "AiAssessments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agreements",
                table: "Agreements",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    EmailVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PhoneVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdentityVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastLivenessVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastTransactionActivityAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ExpiresAt",
                table: "RefreshTokens",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_ProcessedAt",
                table: "OutboxMessages",
                column: "ProcessedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdempotencyKeys_ExpiresAt",
                table: "IdempotencyKeys",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdempotencyKeys_Key_Scope",
                table: "IdempotencyKeys",
                columns: new[] { "Key", "Scope" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisputeEvidence_DisputeId_EvidenceFileId",
                table: "DisputeEvidence",
                columns: new[] { "DisputeId", "EvidenceFileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisputeEvidence_EvidenceFileId",
                table: "DisputeEvidence",
                column: "EvidenceFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DisputeEvidence_SubmittedByUserId",
                table: "DisputeEvidence",
                column: "SubmittedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ActorUserId",
                table: "AuditLogs",
                column: "ActorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityId",
                table: "AuditLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType_Action",
                table: "AuditLogs",
                columns: new[] { "EntityType", "Action" });

            migrationBuilder.CreateIndex(
                name: "IX_VirtualAccounts_AccountNumber",
                table: "VirtualAccounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VirtualAccounts_BusinessId",
                table: "VirtualAccounts",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualAccounts_Status",
                table: "VirtualAccounts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualAccounts_UserId",
                table: "VirtualAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationSteps_VerificationRequestId_Step",
                table: "VerificationSteps",
                columns: new[] { "VerificationRequestId", "Step" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VerificationRequests_RequestedByUserId",
                table: "VerificationRequests",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationRequests_ReviewedBy",
                table: "VerificationRequests",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationRequests_SubjectType_SubjectId",
                table: "VerificationRequests",
                columns: new[] { "SubjectType", "SubjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_VerificationRequests_TransactionId",
                table: "VerificationRequests",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationProviderEvents_ProviderReference",
                table: "VerificationProviderEvents",
                column: "ProviderReference");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationProviderEvents_VerificationRequestId",
                table: "VerificationProviderEvents",
                column: "VerificationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationDocuments_UploadedByUserId",
                table: "VerificationDocuments",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationDocuments_VerificationRequestId",
                table: "VerificationDocuments",
                column: "VerificationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_VectorDocuments_SourceType_SourceId",
                table: "VectorDocuments",
                columns: new[] { "SourceType", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_Key",
                table: "TransactionTypes",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionParties_BusinessId",
                table: "TransactionParties",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionParties_Email",
                table: "TransactionParties",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionParties_TransactionId_UserId",
                table: "TransactionParties",
                columns: new[] { "TransactionId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionParties_UserId",
                table: "TransactionParties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AgreementId",
                table: "Transactions",
                column: "AgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BusinessId",
                table: "Transactions",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedByUserId",
                table: "Transactions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Reference",
                table: "Transactions",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Status_PaymentStatus",
                table: "Transactions",
                columns: new[] { "Status", "PaymentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionTypeId",
                table: "Transactions",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerUserId",
                table: "Reviews",
                column: "ReviewerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TransactionId_ReviewerUserId",
                table: "Reviews",
                columns: new[] { "TransactionId", "ReviewerUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReputationProfiles_SubjectType_SubjectId",
                table: "ReputationProfiles",
                columns: new[] { "SubjectType", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseRequests_RequestedByUserId",
                table: "ReleaseRequests",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseRequests_TransactionId",
                table: "ReleaseRequests",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PayoutAccounts_PartyId",
                table: "PayoutAccounts",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPartnerEvents_ProviderEventId",
                table: "PaymentPartnerEvents",
                column: "ProviderEventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPartnerEvents_VirtualAccountId",
                table: "PaymentPartnerEvents",
                column: "VirtualAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInstructions_IdempotencyKey",
                table: "PaymentInstructions",
                column: "IdempotencyKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInstructions_TransactionId",
                table: "PaymentInstructions",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInstructions_VirtualAccountId",
                table: "PaymentInstructions",
                column: "VirtualAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_BusinessId",
                table: "Parties",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_ReputationProfileId",
                table: "Parties",
                column: "ReputationProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_UserId",
                table: "Parties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipChecks_BusinessId",
                table: "OwnershipChecks",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipChecks_UserId",
                table: "OwnershipChecks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipChecks_VerificationRequestId_BusinessId",
                table: "OwnershipChecks",
                columns: new[] { "VerificationRequestId", "BusinessId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_CreatedAt",
                table: "Notifications",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_ReadAt",
                table: "Notifications",
                columns: new[] { "UserId", "ReadAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_TransactionId",
                table: "Milestones",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_EntryGroupId",
                table: "LedgerEntries",
                column: "EntryGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_TransactionId",
                table: "LedgerEntries",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_TransactionId_EventType",
                table: "LedgerEntries",
                columns: new[] { "TransactionId", "EventType" });

            migrationBuilder.CreateIndex(
                name: "IX_FaceMatchResults_VerificationRequestId",
                table: "FaceMatchResults",
                column: "VerificationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceFiles_MilestoneId",
                table: "EvidenceFiles",
                column: "MilestoneId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceFiles_TransactionId",
                table: "EvidenceFiles",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceFiles_UploadedByUserId",
                table: "EvidenceFiles",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DisputeMessages_DisputeId",
                table: "DisputeMessages",
                column: "DisputeId");

            migrationBuilder.CreateIndex(
                name: "IX_DisputeMessages_SenderUserId",
                table: "DisputeMessages",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_AdminOwnerId",
                table: "Disputes",
                column: "AdminOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_OpenedByUserId",
                table: "Disputes",
                column: "OpenedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Disputes_TransactionId",
                table: "Disputes",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMembers_BusinessId_UserId",
                table: "BusinessMembers",
                columns: new[] { "BusinessId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMembers_UserId",
                table: "BusinessMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_OwnerUserId",
                table: "Businesses",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_RegistrationNumber",
                table: "Businesses",
                column: "RegistrationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_VerificationStatus",
                table: "Businesses",
                column: "VerificationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_AiPromptVersions_Name_Version",
                table: "AiPromptVersions",
                columns: new[] { "Name", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiFeedbacks_AssessmentId",
                table: "AiFeedbacks",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AiFeedbacks_UserId",
                table: "AiFeedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AiAssessments_EntityType_EntityId",
                table: "AiAssessments",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_CreatedByUserId",
                table: "Agreements",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_TransactionId_Version",
                table: "Agreements",
                columns: new[] { "TransactionId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Status",
                table: "Users",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Transactions_TransactionId",
                table: "Agreements",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_CreatedByUserId",
                table: "Agreements",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AiFeedbacks_AiAssessments_AssessmentId",
                table: "AiFeedbacks",
                column: "AssessmentId",
                principalTable: "AiAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AiFeedbacks_Users_UserId",
                table: "AiFeedbacks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Users_OwnerUserId",
                table: "Businesses",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessMembers_Businesses_BusinessId",
                table: "BusinessMembers",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessMembers_Users_UserId",
                table: "BusinessMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DisputeEvidence_Disputes_DisputeId",
                table: "DisputeEvidence",
                column: "DisputeId",
                principalTable: "Disputes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DisputeEvidence_EvidenceFiles_EvidenceFileId",
                table: "DisputeEvidence",
                column: "EvidenceFileId",
                principalTable: "EvidenceFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DisputeEvidence_Users_SubmittedByUserId",
                table: "DisputeEvidence",
                column: "SubmittedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DisputeMessages_Disputes_DisputeId",
                table: "DisputeMessages",
                column: "DisputeId",
                principalTable: "Disputes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DisputeMessages_Users_SenderUserId",
                table: "DisputeMessages",
                column: "SenderUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Disputes_Transactions_TransactionId",
                table: "Disputes",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Disputes_Users_AdminOwnerId",
                table: "Disputes",
                column: "AdminOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Disputes_Users_OpenedByUserId",
                table: "Disputes",
                column: "OpenedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvidenceFiles_Milestones_MilestoneId",
                table: "EvidenceFiles",
                column: "MilestoneId",
                principalTable: "Milestones",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_EvidenceFiles_Transactions_TransactionId",
                table: "EvidenceFiles",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EvidenceFiles_Users_UploadedByUserId",
                table: "EvidenceFiles",
                column: "UploadedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FaceMatchResults_VerificationRequests_VerificationRequestId",
                table: "FaceMatchResults",
                column: "VerificationRequestId",
                principalTable: "VerificationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId",
                table: "LedgerEntries",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Milestones_Transactions_TransactionId",
                table: "Milestones",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnershipChecks_Businesses_BusinessId",
                table: "OwnershipChecks",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnershipChecks_Users_UserId",
                table: "OwnershipChecks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnershipChecks_VerificationRequests_VerificationRequestId",
                table: "OwnershipChecks",
                column: "VerificationRequestId",
                principalTable: "VerificationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Businesses_BusinessId",
                table: "Parties",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_ReputationProfiles_ReputationProfileId",
                table: "Parties",
                column: "ReputationProfileId",
                principalTable: "ReputationProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Users_UserId",
                table: "Parties",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInstructions_Transactions_TransactionId",
                table: "PaymentInstructions",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInstructions_VirtualAccounts_VirtualAccountId",
                table: "PaymentInstructions",
                column: "VirtualAccountId",
                principalTable: "VirtualAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentPartnerEvents_VirtualAccounts_VirtualAccountId",
                table: "PaymentPartnerEvents",
                column: "VirtualAccountId",
                principalTable: "VirtualAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayoutAccounts_Parties_PartyId",
                table: "PayoutAccounts",
                column: "PartyId",
                principalTable: "Parties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReleaseRequests_Transactions_TransactionId",
                table: "ReleaseRequests",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReleaseRequests_Users_RequestedByUserId",
                table: "ReleaseRequests",
                column: "RequestedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Transactions_TransactionId",
                table: "Reviews",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ReviewerUserId",
                table: "Reviews",
                column: "ReviewerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionParties_Businesses_BusinessId",
                table: "TransactionParties",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionParties_Transactions_TransactionId",
                table: "TransactionParties",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionParties_Users_UserId",
                table: "TransactionParties",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Agreements_AgreementId",
                table: "Transactions",
                column: "AgreementId",
                principalTable: "Agreements",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Businesses_BusinessId",
                table: "Transactions",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions",
                column: "TransactionTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_CreatedByUserId",
                table: "Transactions",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationDocuments_Users_UploadedByUserId",
                table: "VerificationDocuments",
                column: "UploadedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationDocuments_VerificationRequests_VerificationRequ~",
                table: "VerificationDocuments",
                column: "VerificationRequestId",
                principalTable: "VerificationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationProviderEvents_VerificationRequests_Verificatio~",
                table: "VerificationProviderEvents",
                column: "VerificationRequestId",
                principalTable: "VerificationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationRequests_Transactions_TransactionId",
                table: "VerificationRequests",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationRequests_Users_RequestedByUserId",
                table: "VerificationRequests",
                column: "RequestedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationRequests_Users_ReviewedBy",
                table: "VerificationRequests",
                column: "ReviewedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationSteps_VerificationRequests_VerificationRequestId",
                table: "VerificationSteps",
                column: "VerificationRequestId",
                principalTable: "VerificationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualAccounts_Businesses_BusinessId",
                table: "VirtualAccounts",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualAccounts_Users_UserId",
                table: "VirtualAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Transactions_TransactionId",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_CreatedByUserId",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_AiFeedbacks_AiAssessments_AssessmentId",
                table: "AiFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_AiFeedbacks_Users_UserId",
                table: "AiFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Users_OwnerUserId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessMembers_Businesses_BusinessId",
                table: "BusinessMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessMembers_Users_UserId",
                table: "BusinessMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_DisputeEvidence_Disputes_DisputeId",
                table: "DisputeEvidence");

            migrationBuilder.DropForeignKey(
                name: "FK_DisputeEvidence_EvidenceFiles_EvidenceFileId",
                table: "DisputeEvidence");

            migrationBuilder.DropForeignKey(
                name: "FK_DisputeEvidence_Users_SubmittedByUserId",
                table: "DisputeEvidence");

            migrationBuilder.DropForeignKey(
                name: "FK_DisputeMessages_Disputes_DisputeId",
                table: "DisputeMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_DisputeMessages_Users_SenderUserId",
                table: "DisputeMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Disputes_Transactions_TransactionId",
                table: "Disputes");

            migrationBuilder.DropForeignKey(
                name: "FK_Disputes_Users_AdminOwnerId",
                table: "Disputes");

            migrationBuilder.DropForeignKey(
                name: "FK_Disputes_Users_OpenedByUserId",
                table: "Disputes");

            migrationBuilder.DropForeignKey(
                name: "FK_EvidenceFiles_Milestones_MilestoneId",
                table: "EvidenceFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_EvidenceFiles_Transactions_TransactionId",
                table: "EvidenceFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_EvidenceFiles_Users_UploadedByUserId",
                table: "EvidenceFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_FaceMatchResults_VerificationRequests_VerificationRequestId",
                table: "FaceMatchResults");

            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Transactions_TransactionId",
                table: "LedgerEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Milestones_Transactions_TransactionId",
                table: "Milestones");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnershipChecks_Businesses_BusinessId",
                table: "OwnershipChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnershipChecks_Users_UserId",
                table: "OwnershipChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnershipChecks_VerificationRequests_VerificationRequestId",
                table: "OwnershipChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Businesses_BusinessId",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_ReputationProfiles_ReputationProfileId",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Users_UserId",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInstructions_Transactions_TransactionId",
                table: "PaymentInstructions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInstructions_VirtualAccounts_VirtualAccountId",
                table: "PaymentInstructions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentPartnerEvents_VirtualAccounts_VirtualAccountId",
                table: "PaymentPartnerEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_PayoutAccounts_Parties_PartyId",
                table: "PayoutAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ReleaseRequests_Transactions_TransactionId",
                table: "ReleaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReleaseRequests_Users_RequestedByUserId",
                table: "ReleaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Transactions_TransactionId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ReviewerUserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionParties_Businesses_BusinessId",
                table: "TransactionParties");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionParties_Transactions_TransactionId",
                table: "TransactionParties");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionParties_Users_UserId",
                table: "TransactionParties");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Agreements_AgreementId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Businesses_BusinessId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionTypes_TransactionTypeId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_CreatedByUserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationDocuments_Users_UploadedByUserId",
                table: "VerificationDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationDocuments_VerificationRequests_VerificationRequ~",
                table: "VerificationDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationProviderEvents_VerificationRequests_Verificatio~",
                table: "VerificationProviderEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationRequests_Transactions_TransactionId",
                table: "VerificationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationRequests_Users_RequestedByUserId",
                table: "VerificationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationRequests_Users_ReviewedBy",
                table: "VerificationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationSteps_VerificationRequests_VerificationRequestId",
                table: "VerificationSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_VirtualAccounts_Businesses_BusinessId",
                table: "VirtualAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_VirtualAccounts_Users_UserId",
                table: "VirtualAccounts");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_ExpiresAt",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_OutboxMessages_ProcessedAt",
                table: "OutboxMessages");

            migrationBuilder.DropIndex(
                name: "IX_IdempotencyKeys_ExpiresAt",
                table: "IdempotencyKeys");

            migrationBuilder.DropIndex(
                name: "IX_IdempotencyKeys_Key_Scope",
                table: "IdempotencyKeys");

            migrationBuilder.DropIndex(
                name: "IX_DisputeEvidence_DisputeId_EvidenceFileId",
                table: "DisputeEvidence");

            migrationBuilder.DropIndex(
                name: "IX_DisputeEvidence_EvidenceFileId",
                table: "DisputeEvidence");

            migrationBuilder.DropIndex(
                name: "IX_DisputeEvidence_SubmittedByUserId",
                table: "DisputeEvidence");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_ActorUserId",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_EntityId",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_EntityType_Action",
                table: "AuditLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VirtualAccounts",
                table: "VirtualAccounts");

            migrationBuilder.DropIndex(
                name: "IX_VirtualAccounts_AccountNumber",
                table: "VirtualAccounts");

            migrationBuilder.DropIndex(
                name: "IX_VirtualAccounts_BusinessId",
                table: "VirtualAccounts");

            migrationBuilder.DropIndex(
                name: "IX_VirtualAccounts_Status",
                table: "VirtualAccounts");

            migrationBuilder.DropIndex(
                name: "IX_VirtualAccounts_UserId",
                table: "VirtualAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationSteps",
                table: "VerificationSteps");

            migrationBuilder.DropIndex(
                name: "IX_VerificationSteps_VerificationRequestId_Step",
                table: "VerificationSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationRequests",
                table: "VerificationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VerificationRequests_RequestedByUserId",
                table: "VerificationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VerificationRequests_ReviewedBy",
                table: "VerificationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VerificationRequests_SubjectType_SubjectId",
                table: "VerificationRequests");

            migrationBuilder.DropIndex(
                name: "IX_VerificationRequests_TransactionId",
                table: "VerificationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationProviderEvents",
                table: "VerificationProviderEvents");

            migrationBuilder.DropIndex(
                name: "IX_VerificationProviderEvents_ProviderReference",
                table: "VerificationProviderEvents");

            migrationBuilder.DropIndex(
                name: "IX_VerificationProviderEvents_VerificationRequestId",
                table: "VerificationProviderEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationDocuments",
                table: "VerificationDocuments");

            migrationBuilder.DropIndex(
                name: "IX_VerificationDocuments_UploadedByUserId",
                table: "VerificationDocuments");

            migrationBuilder.DropIndex(
                name: "IX_VerificationDocuments_VerificationRequestId",
                table: "VerificationDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VectorDocuments",
                table: "VectorDocuments");

            migrationBuilder.DropIndex(
                name: "IX_VectorDocuments_SourceType_SourceId",
                table: "VectorDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionTypes",
                table: "TransactionTypes");

            migrationBuilder.DropIndex(
                name: "IX_TransactionTypes_Key",
                table: "TransactionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AgreementId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BusinessId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CreatedByUserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Reference",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Status_PaymentStatus",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionTypeId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionParties",
                table: "TransactionParties");

            migrationBuilder.DropIndex(
                name: "IX_TransactionParties_BusinessId",
                table: "TransactionParties");

            migrationBuilder.DropIndex(
                name: "IX_TransactionParties_Email",
                table: "TransactionParties");

            migrationBuilder.DropIndex(
                name: "IX_TransactionParties_TransactionId_UserId",
                table: "TransactionParties");

            migrationBuilder.DropIndex(
                name: "IX_TransactionParties_UserId",
                table: "TransactionParties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewerUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_TransactionId_ReviewerUserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReputationProfiles",
                table: "ReputationProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ReputationProfiles_SubjectType_SubjectId",
                table: "ReputationProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReleaseRequests",
                table: "ReleaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReleaseRequests_RequestedByUserId",
                table: "ReleaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReleaseRequests_TransactionId",
                table: "ReleaseRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayoutAccounts",
                table: "PayoutAccounts");

            migrationBuilder.DropIndex(
                name: "IX_PayoutAccounts_PartyId",
                table: "PayoutAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentPartnerEvents",
                table: "PaymentPartnerEvents");

            migrationBuilder.DropIndex(
                name: "IX_PaymentPartnerEvents_ProviderEventId",
                table: "PaymentPartnerEvents");

            migrationBuilder.DropIndex(
                name: "IX_PaymentPartnerEvents_VirtualAccountId",
                table: "PaymentPartnerEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentInstructions",
                table: "PaymentInstructions");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInstructions_IdempotencyKey",
                table: "PaymentInstructions");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInstructions_TransactionId",
                table: "PaymentInstructions");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInstructions_VirtualAccountId",
                table: "PaymentInstructions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parties",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_Parties_BusinessId",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_Parties_ReputationProfileId",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_Parties_UserId",
                table: "Parties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnershipChecks",
                table: "OwnershipChecks");

            migrationBuilder.DropIndex(
                name: "IX_OwnershipChecks_BusinessId",
                table: "OwnershipChecks");

            migrationBuilder.DropIndex(
                name: "IX_OwnershipChecks_UserId",
                table: "OwnershipChecks");

            migrationBuilder.DropIndex(
                name: "IX_OwnershipChecks_VerificationRequestId_BusinessId",
                table: "OwnershipChecks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId_CreatedAt",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId_ReadAt",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Milestones",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Milestones_TransactionId",
                table: "Milestones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LedgerEntries",
                table: "LedgerEntries");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntries_EntryGroupId",
                table: "LedgerEntries");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntries_TransactionId",
                table: "LedgerEntries");

            migrationBuilder.DropIndex(
                name: "IX_LedgerEntries_TransactionId_EventType",
                table: "LedgerEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FaceMatchResults",
                table: "FaceMatchResults");

            migrationBuilder.DropIndex(
                name: "IX_FaceMatchResults_VerificationRequestId",
                table: "FaceMatchResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvidenceFiles",
                table: "EvidenceFiles");

            migrationBuilder.DropIndex(
                name: "IX_EvidenceFiles_MilestoneId",
                table: "EvidenceFiles");

            migrationBuilder.DropIndex(
                name: "IX_EvidenceFiles_TransactionId",
                table: "EvidenceFiles");

            migrationBuilder.DropIndex(
                name: "IX_EvidenceFiles_UploadedByUserId",
                table: "EvidenceFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disputes",
                table: "Disputes");

            migrationBuilder.DropIndex(
                name: "IX_Disputes_AdminOwnerId",
                table: "Disputes");

            migrationBuilder.DropIndex(
                name: "IX_Disputes_OpenedByUserId",
                table: "Disputes");

            migrationBuilder.DropIndex(
                name: "IX_Disputes_TransactionId",
                table: "Disputes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DisputeMessages",
                table: "DisputeMessages");

            migrationBuilder.DropIndex(
                name: "IX_DisputeMessages_DisputeId",
                table: "DisputeMessages");

            migrationBuilder.DropIndex(
                name: "IX_DisputeMessages_SenderUserId",
                table: "DisputeMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessMembers",
                table: "BusinessMembers");

            migrationBuilder.DropIndex(
                name: "IX_BusinessMembers_BusinessId_UserId",
                table: "BusinessMembers");

            migrationBuilder.DropIndex(
                name: "IX_BusinessMembers_UserId",
                table: "BusinessMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Businesses",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_OwnerUserId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_RegistrationNumber",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_VerificationStatus",
                table: "Businesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AiPromptVersions",
                table: "AiPromptVersions");

            migrationBuilder.DropIndex(
                name: "IX_AiPromptVersions_Name_Version",
                table: "AiPromptVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AiFeedbacks",
                table: "AiFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_AiFeedbacks_AssessmentId",
                table: "AiFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_AiFeedbacks_UserId",
                table: "AiFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AiAssessments",
                table: "AiAssessments");

            migrationBuilder.DropIndex(
                name: "IX_AiAssessments_EntityType_EntityId",
                table: "AiAssessments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agreements",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_CreatedByUserId",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_TransactionId_Version",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "VirtualAccounts");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "VirtualAccounts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VirtualAccounts");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "VirtualAccounts");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "ReleaseRequests");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LedgerEntries");

            migrationBuilder.RenameTable(
                name: "VirtualAccounts",
                newName: "VirtualAccount");

            migrationBuilder.RenameTable(
                name: "VerificationSteps",
                newName: "VerificationStep");

            migrationBuilder.RenameTable(
                name: "VerificationRequests",
                newName: "VerificationRequest");

            migrationBuilder.RenameTable(
                name: "VerificationProviderEvents",
                newName: "VerificationProviderEvent");

            migrationBuilder.RenameTable(
                name: "VerificationDocuments",
                newName: "VerificationDocument");

            migrationBuilder.RenameTable(
                name: "VectorDocuments",
                newName: "VectorDocument");

            migrationBuilder.RenameTable(
                name: "TransactionTypes",
                newName: "TransactionType");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameTable(
                name: "TransactionParties",
                newName: "TransactionParty");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "ReputationProfiles",
                newName: "ReputationProfile");

            migrationBuilder.RenameTable(
                name: "ReleaseRequests",
                newName: "ReleaseRequest");

            migrationBuilder.RenameTable(
                name: "PayoutAccounts",
                newName: "PayoutAccount");

            migrationBuilder.RenameTable(
                name: "PaymentPartnerEvents",
                newName: "PaymentPartnerEvent");

            migrationBuilder.RenameTable(
                name: "PaymentInstructions",
                newName: "PaymentInstruction");

            migrationBuilder.RenameTable(
                name: "Parties",
                newName: "Party");

            migrationBuilder.RenameTable(
                name: "OwnershipChecks",
                newName: "OwnershipCheck");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "Milestones",
                newName: "Milestone");

            migrationBuilder.RenameTable(
                name: "LedgerEntries",
                newName: "LedgerEntry");

            migrationBuilder.RenameTable(
                name: "FaceMatchResults",
                newName: "FaceMatchResult");

            migrationBuilder.RenameTable(
                name: "EvidenceFiles",
                newName: "EvidenceFile");

            migrationBuilder.RenameTable(
                name: "Disputes",
                newName: "Dispute");

            migrationBuilder.RenameTable(
                name: "DisputeMessages",
                newName: "DisputeMessage");

            migrationBuilder.RenameTable(
                name: "BusinessMembers",
                newName: "BusinessMember");

            migrationBuilder.RenameTable(
                name: "Businesses",
                newName: "Business");

            migrationBuilder.RenameTable(
                name: "AiPromptVersions",
                newName: "AiPromptVersion");

            migrationBuilder.RenameTable(
                name: "AiFeedbacks",
                newName: "AiFeedback");

            migrationBuilder.RenameTable(
                name: "AiAssessments",
                newName: "AiAssessment");

            migrationBuilder.RenameTable(
                name: "Agreements",
                newName: "Agreement");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "OutboxMessages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "IdempotencyKeys",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestHash",
                table: "IdempotencyKeys",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "IdempotencyKeys",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "VirtualAccount",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "VirtualAccount",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Partner",
                table: "VirtualAccount",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "VirtualAccount",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "VirtualAccount",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "VirtualAccount",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "VirtualAccount",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AmountExpectedMinor",
                table: "VirtualAccount",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "VirtualAccount",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Step",
                table: "VerificationStep",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "VerificationStep",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "VerificationStep",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VerificationType",
                table: "VerificationRequest",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "VerificationLevel",
                table: "VerificationRequest",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectType",
                table: "VerificationRequest",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "VerificationRequest",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "VerificationRequest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "VerificationRequest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "VerificationRequest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentReference",
                table: "VerificationRequest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "VerificationProviderEvent",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "VerificationProviderEvent",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "VerificationProviderEvent",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "VerificationDocument",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "VerificationDocument",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "VerificationDocument",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "VerificationDocument",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "DocumentType",
                table: "VerificationDocument",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SourceType",
                table: "VectorDocument",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "EmbeddingModel",
                table: "VectorDocument",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "RequiredVerificationLevel",
                table: "TransactionType",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ReleaseMode",
                table: "TransactionType",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransactionType",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "TransactionType",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "VerificationLevelRequired",
                table: "Transaction",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Transaction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "RiskLevel",
                table: "Transaction",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "Transaction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "PartyMode",
                table: "Transaction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "Transaction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TransactionParty",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "TransactionParty",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PartyType",
                table: "TransactionParty",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "PartyMode",
                table: "TransactionParty",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "TransactionParty",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(320)",
                oldMaxLength: 320,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "TransactionParty",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "RevieweeSubjectType",
                table: "Review",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectType",
                table: "ReputationProfile",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "RatingAverage",
                table: "ReputationProfile",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ReleaseRequest",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "ReleaseRequest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "ReleaseRequest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderReference",
                table: "PayoutAccount",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NameMatchStatus",
                table: "PayoutAccount",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "PayoutAccount",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankCode",
                table: "PayoutAccount",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumberHash",
                table: "PayoutAccount",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "PayoutAccount",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderEventId",
                table: "PaymentPartnerEvent",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Partner",
                table: "PaymentPartnerEvent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "PaymentPartnerEvent",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PaymentInstruction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SignedPayloadHash",
                table: "PaymentInstruction",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartnerReference",
                table: "PaymentInstruction",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Partner",
                table: "PaymentInstruction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "InstructionType",
                table: "PaymentInstruction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IdempotencyKey",
                table: "PaymentInstruction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "VerificationStatus",
                table: "Party",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartyType",
                table: "Party",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Party",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CacReference",
                table: "Party",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BvnReference",
                table: "Party",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "OwnershipCheck",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Method",
                table: "OwnershipCheck",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Notification",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notification",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Milestone",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Milestone",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "EventType",
                table: "LedgerEntry",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "LedgerEntry",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Account",
                table: "LedgerEntry",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "FaceMatchResult",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<decimal>(
                name: "MatchScore",
                table: "FaceMatchResult",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,4)",
                oldPrecision: 5,
                oldScale: 4);

            migrationBuilder.AlterColumn<int>(
                name: "IdType",
                table: "FaceMatchResult",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IdNumberHash",
                table: "FaceMatchResult",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<decimal>(
                name: "Confidence",
                table: "FaceMatchResult",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,4)",
                oldPrecision: 5,
                oldScale: 4);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "EvidenceFile",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "EvidenceFile",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "EvidenceFile",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "EvidenceFile",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Dispute",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Resolution",
                table: "Dispute",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Dispute",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "BusinessMember",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "BusinessMember",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "VerificationStatus",
                table: "Business",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Business",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TaxId",
                table: "Business",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Business",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RiskLevel",
                table: "Business",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Business",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Business",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Business",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Business",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AiPromptVersion",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AiPromptVersion",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "FeedbackType",
                table: "AiFeedback",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "RiskLevel",
                table: "AiAssessment",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "AiAssessment",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "EntityType",
                table: "AiAssessment",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "AiAssessment",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<decimal>(
                name: "Confidence",
                table: "AiAssessment",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,4)",
                oldPrecision: 5,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssessmentType",
                table: "AiAssessment",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VirtualAccount",
                table: "VirtualAccount",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationStep",
                table: "VerificationStep",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationRequest",
                table: "VerificationRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationProviderEvent",
                table: "VerificationProviderEvent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationDocument",
                table: "VerificationDocument",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VectorDocument",
                table: "VectorDocument",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionType",
                table: "TransactionType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionParty",
                table: "TransactionParty",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReputationProfile",
                table: "ReputationProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReleaseRequest",
                table: "ReleaseRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayoutAccount",
                table: "PayoutAccount",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentPartnerEvent",
                table: "PaymentPartnerEvent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentInstruction",
                table: "PaymentInstruction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Party",
                table: "Party",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnershipCheck",
                table: "OwnershipCheck",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Milestone",
                table: "Milestone",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LedgerEntry",
                table: "LedgerEntry",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FaceMatchResult",
                table: "FaceMatchResult",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvidenceFile",
                table: "EvidenceFile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dispute",
                table: "Dispute",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DisputeMessage",
                table: "DisputeMessage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessMember",
                table: "BusinessMember",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Business",
                table: "Business",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AiPromptVersion",
                table: "AiPromptVersion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AiFeedback",
                table: "AiFeedback",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AiAssessment",
                table: "AiAssessment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agreement",
                table: "Agreement",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    EmailVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    IdentityVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastLivenessVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    LastTransactionActivityAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    PhoneVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }
    }
}
