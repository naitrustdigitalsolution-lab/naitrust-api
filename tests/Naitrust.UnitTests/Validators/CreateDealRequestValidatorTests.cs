using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Naitrust.Application.Validators.Transactions;
using Naitrust.Domain.Models.Dtos.Requests.Transactions;
using Xunit;

namespace Naitrust.UnitTests.Validators;

public class CreateDealRequestValidatorTests
{
    private static readonly CreateDealRequestValidator Validator = new();

    private static CreateDealRequest BaseRequest(
        long? initialPaymentMinor = null,
        string? initialPaymentMode = null,
        int? initialPaymentPercentage = null,
        long? remainingPaymentMinor = null,
        string? nextPaymentReleaseConditions = null,
        List<ParticipantInput>? participants = null)
    {
        return new CreateDealRequest(
            UseCase: "property-agent-payments",
            DealType: "OneTime",
            PartyMode: "b2c",
            Role: "buyer",
            Participants: participants,
            Title: "Test deal",
            Description: "Test description",
            AmountMinor: 1_000_00,
            Currency: "NGN",
            DeliveryDueDate: "2026-09-01",
            ReleaseConditions: "on delivery",
            ExtendedProductTestingDays: null,
            ExpiresInDays: 7,
            Agreement: null,
            InitialPaymentMinor: initialPaymentMinor,
            InitialPaymentMode: initialPaymentMode,
            InitialPaymentPercentage: initialPaymentPercentage,
            RemainingPaymentMinor: remainingPaymentMinor,
            NextPaymentReleaseConditions: nextPaymentReleaseConditions);
    }

    [Fact]
    public void SinglePayment_WithNoStagedFields_IsValid()
    {
        var result = Validator.Validate(BaseRequest());
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void StagedPayment_WithFixedInitialAmountBelowTotal_IsValid()
    {
        var result = Validator.Validate(BaseRequest(
            initialPaymentMinor: 500_00,
            initialPaymentMode: "fixed",
            remainingPaymentMinor: 500_00,
            nextPaymentReleaseConditions: "buyer confirms second milestone"));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void InitialPaymentMinor_EqualToTotal_IsInvalid()
    {
        var result = Validator.Validate(BaseRequest(initialPaymentMinor: 1_000_00));
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDealRequest.InitialPaymentMinor));
    }

    [Theory]
    [InlineData("percentage-based")]
    [InlineData("half")]
    public void InitialPaymentMode_Unrecognized_IsInvalid(string mode)
    {
        var result = Validator.Validate(BaseRequest(initialPaymentMode: mode));
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDealRequest.InitialPaymentMode));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void InitialPaymentPercentage_OutOfRange_IsInvalid(int percentage)
    {
        var result = Validator.Validate(BaseRequest(initialPaymentPercentage: percentage));
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDealRequest.InitialPaymentPercentage));
    }

    [Fact]
    public void ParticipantPaymentAllocation_InvalidStage_IsInvalid()
    {
        var participants = new List<ParticipantInput>
        {
            new("Counterparty", "party@example.com", null, null, null, null,
                new List<PaymentAllocationInput> { new(Stage: 3, AmountMinor: 500_00) })
        };

        var result = Validator.Validate(BaseRequest(participants: participants));
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Stage"));
    }

    [Fact]
    public void ParticipantPaymentAllocation_ValidStages_IsValid()
    {
        var participants = new List<ParticipantInput>
        {
            new("Counterparty", "party@example.com", null, null, null, null,
                new List<PaymentAllocationInput> { new(Stage: 1, AmountMinor: 500_00), new(Stage: 2, AmountMinor: 500_00) })
        };

        var result = Validator.Validate(BaseRequest(participants: participants));
        result.IsValid.Should().BeTrue();
    }
}
