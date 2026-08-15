using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitrust.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DealDeliveryState_DealIdentityCapture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DealDeliveryStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CardToken = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CardOtpCode = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    CardIntendedBuyerUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CardGeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CardExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CardStatus = table.Column<string>(type: "text", nullable: true),
                    CardGeneration = table.Column<int>(type: "integer", nullable: false),
                    CardUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CardInvalidatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HandoverStatus = table.Column<string>(type: "text", nullable: false),
                    HandoverReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HandoverEndsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HandoverCompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HandoverCompletionReason = table.Column<string>(type: "text", nullable: true),
                    FundingReviewStatus = table.Column<string>(type: "text", nullable: false),
                    FundingReviewStartsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FundingReviewEndsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReleaseApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaidOutAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReleaseMethod = table.Column<string>(type: "text", nullable: true),
                    PaymentReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealDeliveryStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealDeliveryStates_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealDeliveryStates_Users_CardIntendedBuyerUserId",
                        column: x => x.CardIntendedBuyerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DealIdentityCaptures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientCaptureId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RepresentativeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BusinessName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Action = table.Column<string>(type: "text", nullable: false),
                    CapturedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VerificationStatus = table.Column<string>(type: "text", nullable: false),
                    EncryptedEvidenceRef = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PhotoAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    LegalHold = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealIdentityCaptures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealIdentityCaptures_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealIdentityCaptures_Users_SubjectUserId",
                        column: x => x.SubjectUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealDeliveryStates_CardIntendedBuyerUserId",
                table: "DealDeliveryStates",
                column: "CardIntendedBuyerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealDeliveryStates_CardToken",
                table: "DealDeliveryStates",
                column: "CardToken");

            migrationBuilder.CreateIndex(
                name: "IX_DealDeliveryStates_TransactionId",
                table: "DealDeliveryStates",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealIdentityCaptures_SubjectUserId",
                table: "DealIdentityCaptures",
                column: "SubjectUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealIdentityCaptures_TransactionId",
                table: "DealIdentityCaptures",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealDeliveryStates");

            migrationBuilder.DropTable(
                name: "DealIdentityCaptures");
        }
    }
}
