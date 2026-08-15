using FluentAssertions;
using Moq;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Payments;
using Naitrust.Domain.Models.Enums.Transactions;
using Xunit;

namespace Naitrust.UnitTests.Services;

/// <summary>Delivery card / handover review / funding review state machine (Tier 3).</summary>
public class DealDeliveryLifecycleTests
{
    private static async Task<Guid> SeedFundedDealAsync(string dbName, long amountMinor = 100_000, long? initialPaymentMinor = null, long? remainingPaymentMinor = null, int? activePaymentStage = null, int? extendedProductTestingDays = null)
    {
        using var seedContext = TestFixture.CreateContext(dbName);
        var seeded = await TestFixture.SeedDealAsync(
            seedContext, status: DealStatus.Funded, paymentStatus: PaymentStatus.PaymentConfirmedByPartner,
            amountMinor: amountMinor, initialPaymentMinor: initialPaymentMinor, remainingPaymentMinor: remainingPaymentMinor,
            activePaymentStage: activePaymentStage, extendedProductTestingDays: extendedProductTestingDays);
        return seeded.Deal.Id;
    }

    private static (Guid buyerId, Guid sellerId) GetParties(string dbName, Guid dealId)
    {
        using var context = TestFixture.CreateContext(dbName);
        var buyer = context.DealParties.First(p => p.DealId == dealId && p.PartyType == PartyType.Buyer);
        var seller = context.DealParties.First(p => p.DealId == dealId && p.PartyType == PartyType.Seller);
        return (buyer.UserId!.Value, seller.UserId!.Value);
    }

    private static async Task SeedDeliveryStateAsync(string dbName, Guid dealId, Action<DealDeliveryState> configure)
    {
        using var context = TestFixture.CreateContext(dbName);
        var state = new DealDeliveryState { Id = Guid.NewGuid(), DealId = dealId, IsActive = true };
        configure(state);
        context.Set<DealDeliveryState>().Add(state);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GenerateDeliveryCardAsync_WhenFundedAndEligible_CreatesActiveCard()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (buyerId, sellerId) = GetParties(dbName, dealId);

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.GenerateDeliveryCardAsync(dealId, sellerId);

        result.Data!.Delivery!.Card.Should().NotBeNull();
        result.Data!.Delivery!.Card!.Status.Should().Be("active");
        result.Data!.Delivery!.Card!.Generation.Should().Be(1);
        result.Data!.Delivery!.Card!.Token.Should().NotBeNullOrEmpty();
        result.Data!.Delivery!.Card!.OtpCode.Should().MatchRegex("^[0-9]{6}$");
    }

    [Fact]
    public async Task GenerateDeliveryCardAsync_NotSeller_ReturnsForbidden()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (buyerId, _) = GetParties(dbName, dealId);

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.GenerateDeliveryCardAsync(dealId, buyerId);

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GenerateDeliveryCardAsync_NotFunded_ReturnsBadRequest()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.AwaitingFunding, paymentStatus: PaymentStatus.NotStarted);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.GenerateDeliveryCardAsync(seeded.Deal.Id, seeded.Seller.UserId!.Value);

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GenerateDeliveryCardAsync_Regenerate_IncrementsGenerationAndReplacesCredentials()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (_, sellerId) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.CardStatus = DeliveryCardStatus.Active;
            s.CardToken = "old-token";
            s.CardOtpCode = "111111";
            s.CardGeneration = 1;
            s.CardGeneratedAt = DateTime.UtcNow.AddHours(-1);
            s.CardExpiresAt = DateTime.UtcNow.AddHours(47);
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.GenerateDeliveryCardAsync(dealId, sellerId);

        result.Data!.Delivery!.Card!.Generation.Should().Be(2);
        result.Data!.Delivery!.Card!.Token.Should().NotBe("old-token");
    }

    [Fact]
    public async Task GenerateDeliveryCardAsync_HandoverAlreadyStarted_ReturnsBadRequest()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (_, sellerId) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s => s.HandoverStatus = HandoverReviewStatus.InProgress);

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.GenerateDeliveryCardAsync(dealId, sellerId);

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task InvalidateDeliveryCardAsync_ActiveCard_SetsInvalidated()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (_, sellerId) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.CardStatus = DeliveryCardStatus.Active;
            s.CardToken = "tok";
            s.CardOtpCode = "222222";
            s.CardExpiresAt = DateTime.UtcNow.AddHours(1);
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.InvalidateDeliveryCardAsync(dealId, sellerId);

        result.Data!.Delivery!.Card!.Status.Should().Be("invalidated");
    }

    [Fact]
    public async Task ConfirmDeliveryReceiptAsync_ValidOtp_StartsHandoverReview()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (buyerId, _) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.CardStatus = DeliveryCardStatus.Active;
            s.CardToken = "tok-123";
            s.CardOtpCode = "654321";
            s.CardExpiresAt = DateTime.UtcNow.AddHours(1);
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ConfirmDeliveryReceiptAsync(dealId, buyerId, new ConfirmDeliveryReceiptRequest(null, "654321"));

        result.Data!.Status.Should().Be(DealStatus.BuyerReview.ToString());
        result.Data!.Delivery!.Handover.Status.Should().Be("in_progress");
        result.Data!.Delivery!.Card!.Status.Should().Be("used");
    }

    [Fact]
    public async Task ConfirmDeliveryReceiptAsync_InvalidCredential_ReturnsBadRequest()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (buyerId, _) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.CardStatus = DeliveryCardStatus.Active;
            s.CardToken = "tok-123";
            s.CardOtpCode = "654321";
            s.CardExpiresAt = DateTime.UtcNow.AddHours(1);
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ConfirmDeliveryReceiptAsync(dealId, buyerId, new ConfirmDeliveryReceiptRequest(null, "000000"));

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ConfirmDeliveryReceiptAsync_WrongIntendedBuyer_ReturnsForbidden()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (buyerId, _) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.CardStatus = DeliveryCardStatus.Active;
            s.CardToken = "tok-123";
            s.CardOtpCode = "654321";
            s.CardExpiresAt = DateTime.UtcNow.AddHours(1);
            s.CardIntendedBuyerUserId = Guid.NewGuid(); // a different buyer account
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ConfirmDeliveryReceiptAsync(dealId, buyerId, new ConfirmDeliveryReceiptRequest(null, "654321"));

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CompleteHandoverReviewAsync_InProgress_StartsFundingReview()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (buyerId, _) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.HandoverStatus = HandoverReviewStatus.InProgress;
            s.HandoverReceivedAt = DateTime.UtcNow;
            s.HandoverEndsAt = DateTime.UtcNow.AddMinutes(10);
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.CompleteHandoverReviewAsync(dealId, buyerId);

        result.Data!.Delivery!.Handover.Status.Should().Be("completed");
        result.Data!.Delivery!.Handover.CompletionReason.Should().Be("buyer_confirmed");
        result.Data!.Delivery!.FundingReview.Status.Should().Be("in_progress");
    }

    [Fact]
    public async Task CompleteHandoverReviewAsync_NotInProgress_ReturnsBadRequest()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (buyerId, _) = GetParties(dbName, dealId);

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.CompleteHandoverReviewAsync(dealId, buyerId);

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ApproveEarlyReleaseAsync_SinglePayment_ReleasesFullAmount_TransitionsToPaidOut()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName, amountMinor: 80_000);
        var (buyerId, _) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.FundingReviewStatus = FundingReviewStatus.InProgress;
            s.FundingReviewStartsAt = DateTime.UtcNow;
            s.FundingReviewEndsAt = DateTime.UtcNow.AddHours(1);
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        TestFixture.SetupTransferSuccess(anchor);
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ApproveEarlyReleaseAsync(dealId, buyerId);

        result.Data!.Status.Should().Be(DealStatus.PaidOut.ToString());
        result.Data!.Delivery!.FundingReview.Status.Should().Be("paid_out");
        result.Data!.Delivery!.FundingReview.ReleaseMethod.Should().Be("buyer_approved");
        anchor.Verify(a => a.InternalTransferAsync(
            It.IsAny<string>(), It.IsAny<string>(), 80_000, It.IsAny<string>(), It.IsAny<string>(), $"release-{dealId}-full", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ApproveEarlyReleaseAsync_StagedStage1_ReleasesOnlyInitialAmount_ActivatesStage2()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName, amountMinor: 100_000, initialPaymentMinor: 60_000, remainingPaymentMinor: 40_000, activePaymentStage: 1);
        var (buyerId, _) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.FundingReviewStatus = FundingReviewStatus.InProgress;
            s.FundingReviewStartsAt = DateTime.UtcNow;
            s.FundingReviewEndsAt = DateTime.UtcNow.AddHours(1);
            s.CardGeneration = 3; // should be reset back to 0 when stage 2 activates
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        TestFixture.SetupTransferSuccess(anchor);
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ApproveEarlyReleaseAsync(dealId, buyerId);

        result.Data!.Status.Should().Be(DealStatus.AwaitingFunding.ToString());
        result.Data!.PaymentStatus.Should().Be(PaymentStatus.AwaitingFunding.ToString());
        result.Data!.ActivePaymentStage.Should().Be(2);
        result.Data!.FirstPaymentReleasedAt.Should().NotBeNull();
        result.Data!.Delivery!.FundingReview.Status.Should().Be("not_started");
        result.Data!.Delivery!.Card.Should().BeNull();
        anchor.Verify(a => a.InternalTransferAsync(
            It.IsAny<string>(), It.IsAny<string>(), 60_000, It.IsAny<string>(), It.IsAny<string>(), $"release-{dealId}-stage1", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ApproveEarlyReleaseAsync_NotInProgress_ReturnsBadRequest()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (buyerId, _) = GetParties(dbName, dealId);

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ApproveEarlyReleaseAsync(dealId, buyerId);

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Reconcile_CardExpiryElapsed_MarksCardExpired()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        var (buyerId, _) = GetParties(dbName, dealId);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.CardStatus = DeliveryCardStatus.Active;
            s.CardToken = "tok";
            s.CardOtpCode = "111111";
            s.CardExpiresAt = DateTime.UtcNow.AddHours(-1); // already expired
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        // Any read-through triggers reconcile; use the confirm-receipt guard path to surface it.
        var result = await orchestrator.ConfirmDeliveryReceiptAsync(dealId, buyerId, new ConfirmDeliveryReceiptRequest(null, "111111"));

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Message.Should().Contain("no longer valid");
    }

    [Fact]
    public async Task Reconcile_HandoverTimerElapsed_CascadesToFundingReviewInProgress()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.HandoverStatus = HandoverReviewStatus.InProgress;
            s.HandoverReceivedAt = DateTime.UtcNow.AddMinutes(-15);
            s.HandoverEndsAt = DateTime.UtcNow.AddMinutes(-5); // elapsed 5 minutes ago
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var deal = context.Deals.First(d => d.Id == dealId);
        var delivery = await orchestrator.ReconcileAndGetDeliveryStateAsync(deal);

        delivery.Handover.Status.Should().Be("completed");
        delivery.Handover.CompletionReason.Should().Be("timer_elapsed");
        delivery.FundingReview.Status.Should().Be("in_progress");
    }

    [Fact]
    public async Task Reconcile_FundingReviewTimerElapsed_ReleasesFundsAutomaticallyAndPaysOut()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName, amountMinor: 30_000);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.FundingReviewStatus = FundingReviewStatus.InProgress;
            s.FundingReviewStartsAt = DateTime.UtcNow.AddHours(-2);
            s.FundingReviewEndsAt = DateTime.UtcNow.AddHours(-1); // elapsed an hour ago
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        TestFixture.SetupTransferSuccess(anchor);
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var deal = context.Deals.First(d => d.Id == dealId);
        var delivery = await orchestrator.ReconcileAndGetDeliveryStateAsync(deal);

        delivery.FundingReview.Status.Should().Be("paid_out");
        delivery.FundingReview.ReleaseMethod.Should().Be("automatic");
        anchor.Verify(a => a.InternalTransferAsync(
            It.IsAny<string>(), It.IsAny<string>(), 30_000, It.IsAny<string>(), It.IsAny<string>(), $"release-{dealId}-full", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Reconcile_NoDeliveryStateRow_ReturnsEmptyNotStartedLifecycle()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var deal = context.Deals.First(d => d.Id == dealId);
        var delivery = await orchestrator.ReconcileAndGetDeliveryStateAsync(deal);

        delivery.Card.Should().BeNull();
        delivery.Handover.Status.Should().Be("not_started");
        delivery.FundingReview.Status.Should().Be("not_started");
    }

    [Fact]
    public async Task BlockDeliveryReleaseAsync_HandoverInProgress_MarksIssueReportedAndBlocksFundingReview()
    {
        var dbName = TestFixture.NewDbName();
        var dealId = await SeedFundedDealAsync(dbName);
        await SeedDeliveryStateAsync(dbName, dealId, s =>
        {
            s.HandoverStatus = HandoverReviewStatus.InProgress;
            s.HandoverReceivedAt = DateTime.UtcNow;
            s.HandoverEndsAt = DateTime.UtcNow.AddMinutes(10);
        });

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        await orchestrator.BlockDeliveryReleaseAsync(dealId);

        using var assertContext = TestFixture.CreateContext(dbName);
        var deal = assertContext.Deals.First(d => d.Id == dealId);
        deal.Status.Should().Be(DealStatus.Disputed);
        var state = assertContext.Set<DealDeliveryState>().First(s => s.DealId == dealId);
        state.HandoverStatus.Should().Be(HandoverReviewStatus.IssueReported);
        state.FundingReviewStatus.Should().Be(FundingReviewStatus.Blocked);
    }
}
