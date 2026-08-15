using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Naitrust.Application.Services.Implementations.Disputes;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Requests.Disputes;
using Naitrust.Domain.Models.Entities;
using Naitrust.Domain.Models.Enums.Disputes;
using Naitrust.Domain.Models.Enums.Payments;
using Naitrust.Domain.Models.Enums.Transactions;
using Xunit;

namespace Naitrust.UnitTests.Services;

public class DisputeServiceTests
{
    private static Mock<UserManager<NaitrustUser>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<NaitrustUser>>();
        return new Mock<UserManager<NaitrustUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    private static DisputeService CreateService(Naitrust.Infrastructure.Context.NaitrustDbContext context, Mock<IDealOrchestrator> orchestrator, Mock<UserManager<NaitrustUser>>? userManager = null)
    {
        var unitOfWork = TestFixture.CreateUnitOfWork(context);
        return new DisputeService(unitOfWork, (userManager ?? CreateUserManagerMock()).Object, orchestrator.Object);
    }

    [Fact]
    public async Task OpenDisputeAsync_WithEvidence_OpensUnderReview_AndBlocksDeliveryRelease()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.InProgress, paymentStatus: PaymentStatus.PaymentConfirmedByPartner);
        }

        using var context = TestFixture.CreateContext(dbName);
        var orchestrator = new Mock<IDealOrchestrator>();
        var service = CreateService(context, orchestrator);

        var result = await service.OpenDisputeAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value, new OpenDisputeRequest("Item not as described", "Details here", HasEvidence: true));

        result.Data!.Status.Should().Be("under_review");
        orchestrator.Verify(o => o.BlockDeliveryReleaseAsync(seeded.Deal.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OpenDisputeAsync_WithoutEvidence_OpensAwaitingEvidence_DoesNotBlockDeliveryRelease()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.InProgress, paymentStatus: PaymentStatus.PaymentConfirmedByPartner);
        }

        using var context = TestFixture.CreateContext(dbName);
        var orchestrator = new Mock<IDealOrchestrator>();
        var service = CreateService(context, orchestrator);

        var result = await service.OpenDisputeAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value, new OpenDisputeRequest("Item not as described", null, HasEvidence: false));

        result.Data!.Status.Should().Be("awaiting_evidence");
        orchestrator.Verify(o => o.BlockDeliveryReleaseAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task OpenDisputeAsync_DealNotFound_ReturnsNotFound()
    {
        using var context = TestFixture.CreateContext();
        var orchestrator = new Mock<IDealOrchestrator>();
        var service = CreateService(context, orchestrator);

        var result = await service.OpenDisputeAsync(Guid.NewGuid(), Guid.NewGuid(), new OpenDisputeRequest("Reason", null, false));

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task OpenDisputeAsync_AlreadyOpenDispute_ReturnsBadRequest_DoesNotBlockAgain()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.InProgress, paymentStatus: PaymentStatus.PaymentConfirmedByPartner);
            seedContext.Disputes.Add(new Dispute { Id = Guid.NewGuid(), DealId = seeded.Deal.Id, OpenedByUserId = seeded.Buyer.UserId!.Value, Status = DisputeStatus.UnderReview, Reason = "Prior issue", IsActive = true });
            await seedContext.SaveChangesAsync();
        }

        using var context = TestFixture.CreateContext(dbName);
        var orchestrator = new Mock<IDealOrchestrator>();
        var service = CreateService(context, orchestrator);

        var result = await service.OpenDisputeAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value, new OpenDisputeRequest("Another issue", null, true));

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        orchestrator.Verify(o => o.BlockDeliveryReleaseAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddMessageToTransactionDisputeAsync_AddsMessage()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        Guid disputeId;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.InProgress, paymentStatus: PaymentStatus.PaymentConfirmedByPartner);
            var dispute = new Dispute { Id = Guid.NewGuid(), DealId = seeded.Deal.Id, OpenedByUserId = seeded.Buyer.UserId!.Value, Status = DisputeStatus.UnderReview, Reason = "Issue", IsActive = true };
            seedContext.Disputes.Add(dispute);
            await seedContext.SaveChangesAsync();
            disputeId = dispute.Id;
        }

        using var context = TestFixture.CreateContext(dbName);
        var orchestrator = new Mock<IDealOrchestrator>();
        var service = CreateService(context, orchestrator);

        var result = await service.AddMessageToTransactionDisputeAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value, new AddDisputeMessageRequest("Here is more evidence"));

        result.Data!.Messages.Should().Contain(m => m.Body == "Here is more evidence" && m.ByYou);

        using var assertContext = TestFixture.CreateContext(dbName);
        assertContext.DisputeMessages.Count(m => m.DisputeId == disputeId).Should().Be(1);
    }

    [Fact]
    public async Task GetByTransactionAsync_NoDispute_ReturnsSuccessWithNullData()
    {
        var dbName = TestFixture.NewDbName();
        TestFixture.SeededDeal seeded;
        using (var seedContext = TestFixture.CreateContext(dbName))
        {
            seeded = await TestFixture.SeedDealAsync(seedContext, status: DealStatus.InProgress, paymentStatus: PaymentStatus.PaymentConfirmedByPartner);
        }

        using var context = TestFixture.CreateContext(dbName);
        var orchestrator = new Mock<IDealOrchestrator>();
        var service = CreateService(context, orchestrator);

        var result = await service.GetByTransactionAsync(seeded.Deal.Id, seeded.Buyer.UserId!.Value);

        result.Data.Should().BeNull();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
