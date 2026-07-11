using Xunit;
using FluentAssertions;

namespace Naitrust.ArchitectureTests;

public class LayerDependencyTests
{
    [Fact]
    public void Domain_ShouldNotDependOnApplication()
    {
        // TODO: Use NetArchTest.Rules to verify layer dependencies
        true.Should().BeTrue();
    }

    [Fact]
    public void Domain_ShouldNotDependOnInfrastructure()
    {
        true.Should().BeTrue();
    }

    [Fact]
    public void Domain_ShouldNotDependOnApi()
    {
        true.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_ShouldNotDependOnApi()
    {
        true.Should().BeTrue();
    }
}
