using FluentAssertions;
using Moq;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Payments;
using Naitrust.Domain.Models.Enums.Transactions;
using Xunit;

namespace Naitrust.UnitTests.Services;

public class DealOrchestratorTests
{
    [Fact]
    public async Task InvitePartyAsync_FromDraft_CreatesInvitationAndTransitionsToPendingCounterparty()
    {
        var dbName = TestFixture.NewDbName();
        var buyerId = Guid.NewGuid();
        Guid dealId;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            var deal = new Deal { Id = Guid.NewGuid(), Reference = "NTR-TEST0001", DealType = DealType.Single, CreatedByUserId = buyerId, PartyMode = PartyMode.B2C, Title = "Deal", Category = DealCategory.Other, AmountMinor = 10_000, Currency = "NGN", Status = DealStatus.Draft, PaymentStatus = PaymentStatus.NotStarted, IsActive = true };
            seedContext.Deals.Add(deal);
            seedContext.DealParties.Add(new DealParty { Id = Guid.NewGuid(), DealId = deal.Id, UserId = buyerId, PartyType = PartyType.Buyer, PartyMode = PartyMode.B2C, DisplayName = "Buyer", Status = DealPartyStatus.Accepted, IsActive = true });
            await seedContext.SaveChangesAsync();
            dealId = deal.Id;
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.InvitePartyAsync(dealId, buyerId, new InvitePartyRequest("seller@example.com", null, "Seller", "Seller Co"));

        result.Data!.Status.Should().Be(DealStatus.PendingCounterparty.ToString());

        using var assertContext = TestFixture.CreateContext(dbName);
        assertContext.DealParties.Count(p => p.DealId == dealId && p.PartyType == PartyType.Seller).Should().Be(1);
        assertContext.DealInvitations.Count(i => i.DealId == dealId).Should().Be(1);
    }

    [Fact]
    public async Task InvitePartyAsync_NotDraft_ReturnsBadRequest()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.Funded);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.InvitePartyAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value, new InvitePartyRequest("x@example.com", null, "Seller", null));

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AcceptInvitationAsync_FromPendingCounterparty_TransitionsToTermsNegotiation()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.PendingCounterparty);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.AcceptInvitationAsync(seeded.Deal.Id, seeded.Seller.UserId!.Value);

        result.Data!.Status.Should().Be(DealStatus.TermsNegotiation.ToString());
    }

    [Fact]
    public async Task RejectInvitationAsync_FromPendingCounterparty_TransitionsToCancelled()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.PendingCounterparty);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.RejectInvitationAsync(seeded.Deal.Id, seeded.Seller.UserId!.Value);

        result.Data!.Status.Should().Be(DealStatus.Cancelled.ToString());
    }

    [Fact]
    public async Task ProposeTermsAsync_CreatesAgreement_LinksToDeal()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.TermsNegotiation);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ProposeTermsAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value,
            new ProposeTermsRequest("Summary", "Description", null, "On delivery", null, null, null, null, null, false));

        result.Data!.Agreement.Should().NotBeNull();

        using var assertContext = TestFixture.CreateContext(dbName);
        assertContext.Agreements.Count(a => a.DealId == seeded.Deal.Id).Should().Be(1);
    }

    [Fact]
    public async Task ApproveTermsAsync_FreezesAgreement_TransitionsToAwaitingFunding()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        Guid agreementId;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.TermsNegotiation);
            var agreement = new Agreement { Id = Guid.NewGuid(), DealId = seeded.Deal.Id, Version = 1, CreatedByUserId = seeded.Buyer.UserId!.Value, IsActive = true };
            seedContext.Agreements.Add(agreement);
            var deal = await seedContext.Deals.FindAsync(seeded.Deal.Id);
            deal!.AgreementId = agreement.Id;
            await seedContext.SaveChangesAsync();
            agreementId = agreement.Id;
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ApproveTermsAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value);

        result.Data!.Status.Should().Be(DealStatus.AwaitingFunding.ToString());

        using var assertContext = TestFixture.CreateContext(dbName);
        (await assertContext.Agreements.FindAsync(agreementId))!.FrozenAt.Should().NotBeNull();
    }

    [Fact]
    public async Task InitiateFundingAsync_SinglePayment_TransfersFullAmount_TransitionsToFunded()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.AwaitingFunding, amountMinor: 100_000);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        TestFixture.SetupSufficientBalance(anchor, 100_000);
        TestFixture.SetupTransferSuccess(anchor);
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.InitiateFundingAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value);

        result.Data!.Status.Should().Be(DealStatus.Funded.ToString());
        result.Data!.PaymentStatus.Should().Be(PaymentStatus.PaymentConfirmedByPartner.ToString());
        anchor.Verify(a => a.InternalTransferAsync(
            seeded.BuyerWallet.ProviderReference!, seeded.PlatformEscrow.ProviderReference!, 100_000, "NGN", It.IsAny<string>(), $"fund-{seeded.Deal.Id}", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InitiateFundingAsync_InsufficientBalance_ReturnsBadRequest_DoesNotTransfer()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.AwaitingFunding, amountMinor: 100_000);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        TestFixture.SetupSufficientBalance(anchor, 5_000);
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.InitiateFundingAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value);

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        anchor.Verify(a => a.InternalTransferAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task InitiateFundingAsync_NotBuyer_ReturnsForbidden()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.AwaitingFunding);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.InitiateFundingAsync(seeded.Deal.Id, seeded.Seller.UserId!.Value);

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task InitiateFundingAsync_StagedStage1_TransfersOnlyInitialAmount()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.AwaitingFunding, amountMinor: 100_000, initialPaymentMinor: 60_000, remainingPaymentMinor: 40_000, activePaymentStage: 1);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        TestFixture.SetupSufficientBalance(anchor, 60_000);
        TestFixture.SetupTransferSuccess(anchor);
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        await orchestrator.InitiateFundingAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value);

        anchor.Verify(a => a.InternalTransferAsync(
            It.IsAny<string>(), It.IsAny<string>(), 60_000, It.IsAny<string>(), It.IsAny<string>(), $"fund-{seeded.Deal.Id}", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InitiateFundingAsync_StagedStage2_TransfersRemainingAmount_WithDistinctIdempotencyKey()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.AwaitingFunding, amountMinor: 100_000, initialPaymentMinor: 60_000, remainingPaymentMinor: 40_000, activePaymentStage: 2);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        TestFixture.SetupSufficientBalance(anchor, 40_000);
        TestFixture.SetupTransferSuccess(anchor);
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        await orchestrator.InitiateFundingAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value);

        anchor.Verify(a => a.InternalTransferAsync(
            It.IsAny<string>(), It.IsAny<string>(), 40_000, It.IsAny<string>(), It.IsAny<string>(), $"fund-{seeded.Deal.Id}-stage2", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SubmitDeliveryAsync_FromFunded_TransitionsToInProgress()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.Funded, paymentStatus: PaymentStatus.PaymentConfirmedByPartner);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.SubmitDeliveryAsync(seeded.Deal.Id, seeded.Seller.UserId!.Value);

        result.Data!.Status.Should().Be(DealStatus.InProgress.ToString());
    }

    [Fact]
    public async Task ConfirmDeliveryAsync_ReleasesFullAmountToSeller_TransitionsToCompleted()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.InProgress, paymentStatus: PaymentStatus.PaymentConfirmedByPartner, amountMinor: 75_000);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        TestFixture.SetupTransferSuccess(anchor);
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ConfirmDeliveryAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value);

        result.Data!.Status.Should().Be(DealStatus.Completed.ToString());
        result.Data!.PaymentStatus.Should().Be(PaymentStatus.Released.ToString());
        anchor.Verify(a => a.InternalTransferAsync(
            seeded.PlatformEscrow.ProviderReference!, seeded.SellerWallet.ProviderReference!, 75_000, It.IsAny<string>(), It.IsAny<string>(), $"release-{seeded.Deal.Id}", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ConfirmDeliveryAsync_NotBuyer_ReturnsForbidden()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.InProgress, paymentStatus: PaymentStatus.PaymentConfirmedByPartner);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.ConfirmDeliveryAsync(seeded.Deal.Id, seeded.Seller.UserId!.Value);

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CancelDealAsync_WhenFunded_RefundsBuyer()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.Funded, paymentStatus: PaymentStatus.PaymentConfirmedByPartner, amountMinor: 50_000);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        TestFixture.SetupTransferSuccess(anchor);
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.CancelDealAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value);

        result.Data!.Status.Should().Be(DealStatus.Cancelled.ToString());
        anchor.Verify(a => a.InternalTransferAsync(
            seeded.PlatformEscrow.ProviderReference!, seeded.BuyerWallet.ProviderReference!, 50_000, It.IsAny<string>(), It.IsAny<string>(), $"refund-{seeded.Deal.Id}", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CancelDealAsync_AlreadyCompleted_ReturnsBadRequest()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.Completed);
        }

        using var context = TestFixture.CreateContext(dbName);
        var anchor = TestFixture.CreateAnchorMock();
        var orchestrator = TestFixture.CreateOrchestrator(context, anchor);

        var result = await orchestrator.CancelDealAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value);

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
