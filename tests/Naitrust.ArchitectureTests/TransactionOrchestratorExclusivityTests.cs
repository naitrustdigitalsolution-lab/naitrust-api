using Xunit;
using FluentAssertions;

namespace Naitrust.ArchitectureTests;

public class TransactionOrchestratorExclusivityTests
{
    [Fact]
    public void OnlyTransactionOrchestrator_ShouldChangeTransactionStatus()
    {
        // TODO: Verify that only TransactionOrchestrator can change TransactionStatus
        true.Should().BeTrue();
    }
}
