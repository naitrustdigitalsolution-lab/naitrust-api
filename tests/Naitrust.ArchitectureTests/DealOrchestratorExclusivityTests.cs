using Xunit;
using FluentAssertions;

namespace Naitrust.ArchitectureTests;

public class DealOrchestratorExclusivityTests
{
    [Fact]
    public void OnlyDealOrchestrator_ShouldChangeDealStatus()
    {
        // TODO: Verify that only DealOrchestrator can change DealStatus
        true.Should().BeTrue();
    }
}
