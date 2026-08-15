using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Naitrust.Application.ExternalServices;
using Naitrust.Application.ExternalServices.Anchor;
using Naitrust.Application.Services.Implementations.Transactions;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Configurations.ConfigModels;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums;
using Naitrust.Domain.Models.Enums.Payments;
using Naitrust.Domain.Models.Enums.Transactions;
using Naitrust.Infrastructure.Context;
using Naitrust.Infrastructure.Data.Implementations;

namespace Naitrust.UnitTests.Services;

/// <summary>
/// Shared test infrastructure: a real EF Core InMemory-backed NaitrustDbContext + UnitOfWork
/// (so repository queries/predicates run for real), with only the true external boundary —
/// AnchorPaymentPartner and INotificationService — mocked.
/// </summary>
public static class TestFixture
{
    /// <summary>
    /// Each call returns a fresh NaitrustDbContext instance (own change tracker), optionally pointed
    /// at a shared in-memory database name so multiple contexts see the same data — mirroring how a
    /// real request gets its own DbContext against the same physical database. Never reuse one
    /// context instance across "seed" and "act" steps: EF's change tracker will collide when the
    /// no-tracking repository re-fetches an entity that's still tracked from the seed step.
    /// </summary>
    public static NaitrustDbContext CreateContext(string? dbName = null) => new(
        new DbContextOptionsBuilder<NaitrustDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options);

    public static string NewDbName() => Guid.NewGuid().ToString();

    public static UnitOfWork<NaitrustDbContext> CreateUnitOfWork(NaitrustDbContext context) => new(context);

    public static Mock<AnchorPaymentPartner> CreateAnchorMock() => new(
        new HttpClient(),
        Options.Create(new AnchorSettings()),
        NullLogger<AnchorPaymentPartner>.Instance);

    public static void SetupSufficientBalance(Mock<AnchorPaymentPartner> anchor, long amountReceivedMinor) =>
        anchor.Setup(a => a.GetFundingStatusAsync(It.IsAny<FundingStatusRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FundingStatusResult("funded", amountReceivedMinor));

    public static void SetupTransferSuccess(Mock<AnchorPaymentPartner> anchor, string partnerReference = "anchor-ref") =>
        anchor.Setup(a => a.InternalTransferAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PaymentInstructionResult(partnerReference, "confirmed"));

    public static DealOrchestrator CreateOrchestrator(NaitrustDbContext context, Mock<AnchorPaymentPartner> anchor) =>
        new(CreateUnitOfWork(context), new Mock<INotificationService>().Object, anchor.Object);

    public record SeededDeal(Deal Deal, DealParty Buyer, DealParty Seller, VirtualAccount BuyerWallet, VirtualAccount SellerWallet, VirtualAccount PlatformEscrow);

    /// <summary>Seeds a Deal with accepted buyer/seller parties and issued virtual accounts (buyer, seller, platform escrow).</summary>
    public static async Task<SeededDeal> SeedDealAsync(
        NaitrustDbContext context,
        DealStatus status = DealStatus.AwaitingFunding,
        PaymentStatus paymentStatus = PaymentStatus.NotStarted,
        long amountMinor = 100_000,
        long? initialPaymentMinor = null,
        long? remainingPaymentMinor = null,
        int? activePaymentStage = null,
        int? extendedProductTestingDays = null)
    {
        var buyerId = Guid.NewGuid();
        var sellerId = Guid.NewGuid();

        var deal = new Deal
        {
            Id = Guid.NewGuid(),
            Reference = $"NTR-{Guid.NewGuid():N}".Substring(0, 12).ToUpper(),
            DealType = DealType.Single,
            CreatedByUserId = buyerId,
            PartyMode = PartyMode.B2C,
            Title = "Test deal",
            Category = DealCategory.Other,
            AmountMinor = amountMinor,
            Currency = "NGN",
            Status = status,
            PaymentStatus = paymentStatus,
            InitialPaymentMinor = initialPaymentMinor,
            RemainingPaymentMinor = remainingPaymentMinor,
            ActivePaymentStage = activePaymentStage,
            ExtendedProductTestingDays = extendedProductTestingDays,
            IsActive = true
        };
        context.Deals.Add(deal);

        var buyer = new DealParty
        {
            Id = Guid.NewGuid(), DealId = deal.Id, UserId = buyerId, PartyType = PartyType.Buyer,
            PartyMode = PartyMode.B2C, DisplayName = "Buyer", Status = DealPartyStatus.Accepted, IsActive = true
        };
        var seller = new DealParty
        {
            Id = Guid.NewGuid(), DealId = deal.Id, UserId = sellerId, PartyType = PartyType.Seller,
            PartyMode = PartyMode.B2C, DisplayName = "Seller", Status = DealPartyStatus.Accepted, IsActive = true
        };
        context.DealParties.AddRange(buyer, seller);

        var buyerWallet = new VirtualAccount
        {
            Id = Guid.NewGuid(), UserId = buyerId, Type = VirtualAccountType.Settlement, Partner = PaymentPartnerId.Anchor,
            ProviderReference = $"buyer-sub-{buyerId}", Currency = "NGN", Status = VirtualAccountStatus.Issued, IsActive = true
        };
        var sellerWallet = new VirtualAccount
        {
            Id = Guid.NewGuid(), UserId = sellerId, Type = VirtualAccountType.Settlement, Partner = PaymentPartnerId.Anchor,
            ProviderReference = $"seller-sub-{sellerId}", Currency = "NGN", Status = VirtualAccountStatus.Issued, IsActive = true
        };
        var platformEscrow = new VirtualAccount
        {
            Id = Guid.NewGuid(), Type = VirtualAccountType.Platform, Partner = PaymentPartnerId.Anchor,
            ProviderReference = "platform-escrow", Currency = "NGN", Status = VirtualAccountStatus.Issued, IsActive = true
        };
        context.VirtualAccounts.AddRange(buyerWallet, sellerWallet, platformEscrow);

        await context.SaveChangesAsync();
        return new SeededDeal(deal, buyer, seller, buyerWallet, sellerWallet, platformEscrow);
    }
}
