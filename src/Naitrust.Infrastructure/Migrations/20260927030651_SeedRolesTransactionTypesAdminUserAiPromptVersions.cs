using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Naitrust.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesTransactionTypesAdminUserAiPromptVersions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AiPromptVersions",
                columns: new[] { "Id", "CreatedAt", "IsActive", "IsDeleted", "Name", "Purpose", "Schema", "Status", "UpdatedAt", "Version" },
                values: new object[,]
                {
                    { new Guid("b1b2c3d4-e5f6-7890-abcd-ef1234567801"), new DateTime(2026, 9, 27, 3, 6, 49, 873, DateTimeKind.Utc).AddTicks(372), true, false, "transaction_risk_assessment", "Assess risk level for a transaction based on parties, amount, category, and verification status", null, "Draft", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { new Guid("b1b2c3d4-e5f6-7890-abcd-ef1234567802"), new DateTime(2026, 9, 27, 3, 6, 49, 873, DateTimeKind.Utc).AddTicks(376), true, false, "dispute_summary", "Generate a neutral dispute summary for admin review", null, "Draft", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { new Guid("b1b2c3d4-e5f6-7890-abcd-ef1234567803"), new DateTime(2026, 9, 27, 3, 6, 49, 873, DateTimeKind.Utc).AddTicks(377), true, false, "evidence_checklist", "Generate evidence completeness checklist for a transaction category", null, "Draft", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { new Guid("b1b2c3d4-e5f6-7890-abcd-ef1234567804"), new DateTime(2026, 9, 27, 3, 6, 49, 873, DateTimeKind.Utc).AddTicks(379), true, false, "verification_mismatch_summary", "Summarize verification mismatches for admin review", null, "Draft", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000010"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full system access with all administrative privileges", "SuperAdmin", "SUPERADMIN" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Administrative access for managing users and transactions", "Admin", "ADMIN" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Standard user with access to create and participate in transactions", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Id", "AutoConfirmWindowHours", "CreatedAt", "DisputeRules", "EvidenceRequirements", "FeeModel", "IsActive", "IsDeleted", "Key", "Name", "ReleaseMode", "RequiredVerificationLevel", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567801"), 72, new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(7840), null, null, null, true, false, "import", "Import Transaction", "Single", "Enhanced", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567802"), 72, new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(7845), null, null, null, true, false, "export", "Export Transaction", "Single", "Enhanced", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567803"), 48, new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(7847), null, null, null, true, false, "freelance", "Freelance Project", "Single", "Standard", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567804"), 168, new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(7849), null, null, null, true, false, "real_estate", "Real Estate Transaction", "Milestone", "Enhanced", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567805"), 48, new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(7851), null, null, null, true, false, "general_goods", "General Goods Purchase", "Single", "Basic", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567806"), 48, new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(7858), null, null, null, true, false, "service", "Service Agreement", "Single", "Standard", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567807"), 72, new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(7860), null, null, null, true, false, "vehicle", "Vehicle Purchase", "Single", "Enhanced", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AnchorCustomerId", "Avatar", "Bio", "City", "ConcurrencyStamp", "Country", "CreatedAt", "Email", "EmailConfirmed", "EmailVerifiedAt", "FirstName", "IdentityVerifiedAt", "IsActive", "IsDeleted", "KycLevel", "LastLivenessVerifiedAt", "LastName", "LastTransactionActivityAt", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhoneVerifiedAt", "PinHash", "SecurityStamp", "State", "Status", "TotpSecret", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), 0, null, null, null, null, null, "a93a4a13-f415-4420-8164-6be9c5c17156", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@naitrust.com", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Naitrust", null, true, false, null, null, "Admin", null, false, null, "ADMIN@NAITRUST.COM", "ADMIN@NAITRUST.COM", "AQAAAAIAAYagAAAAEPCI0l0XAufqZ4uRllXVEdDAXofR++T0enIomCJygSYKaE/hiI1W3MEquFIc7l8FVg==", null, false, null, null, "6c783924-e97d-4a0d-9bb6-847164116e89", null, "Active", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@naitrust.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "CreatedAt", "IsActive", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Permission", "Users.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5045), true, new Guid("00000000-0000-0000-0000-000000000010"), null },
                    { 2, "Permission", "Transactions.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5049), true, new Guid("00000000-0000-0000-0000-000000000010"), null },
                    { 3, "Permission", "Disputes.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5050), true, new Guid("00000000-0000-0000-0000-000000000010"), null },
                    { 4, "Permission", "Verifications.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5051), true, new Guid("00000000-0000-0000-0000-000000000010"), null },
                    { 5, "Permission", "Payments.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5053), true, new Guid("00000000-0000-0000-0000-000000000010"), null },
                    { 6, "Permission", "Reports.View", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5054), true, new Guid("00000000-0000-0000-0000-000000000010"), null },
                    { 7, "Permission", "Settings.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5055), true, new Guid("00000000-0000-0000-0000-000000000010"), null },
                    { 8, "Permission", "AI.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5056), true, new Guid("00000000-0000-0000-0000-000000000010"), null },
                    { 9, "Permission", "Users.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5057), true, new Guid("00000000-0000-0000-0000-000000000011"), null },
                    { 10, "Permission", "Transactions.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5068), true, new Guid("00000000-0000-0000-0000-000000000011"), null },
                    { 11, "Permission", "Disputes.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5069), true, new Guid("00000000-0000-0000-0000-000000000011"), null },
                    { 12, "Permission", "Verifications.Manage", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5071), true, new Guid("00000000-0000-0000-0000-000000000011"), null },
                    { 13, "Permission", "Reports.View", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5072), true, new Guid("00000000-0000-0000-0000-000000000011"), null },
                    { 14, "Permission", "Transactions.Own", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5073), true, new Guid("00000000-0000-0000-0000-000000000012"), null },
                    { 15, "Permission", "Disputes.Own", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5074), true, new Guid("00000000-0000-0000-0000-000000000012"), null },
                    { 16, "Permission", "Verifications.Own", new DateTime(2026, 9, 27, 3, 6, 49, 806, DateTimeKind.Utc).AddTicks(5075), true, new Guid("00000000-0000-0000-0000-000000000012"), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AiPromptVersions",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-e5f6-7890-abcd-ef1234567801"));

            migrationBuilder.DeleteData(
                table: "AiPromptVersions",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-e5f6-7890-abcd-ef1234567802"));

            migrationBuilder.DeleteData(
                table: "AiPromptVersions",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-e5f6-7890-abcd-ef1234567803"));

            migrationBuilder.DeleteData(
                table: "AiPromptVersions",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-e5f6-7890-abcd-ef1234567804"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567801"));

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567802"));

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567803"));

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567804"));

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567805"));

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567806"));

            migrationBuilder.DeleteData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567807"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
