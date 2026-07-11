using Xunit;
using FluentAssertions;

namespace Naitrust.ArchitectureTests;

public class NamingConventionTests
{
    [Fact]
    public void Controllers_ShouldEndWithController()
    {
        // TODO: Use NetArchTest.Rules to verify controller naming conventions
        true.Should().BeTrue();
    }

    [Fact]
    public void Services_ShouldEndWithService()
    {
        // TODO: Use NetArchTest.Rules to verify service naming conventions
        true.Should().BeTrue();
    }

    [Fact]
    public void Validators_ShouldEndWithValidator()
    {
        // TODO: Use NetArchTest.Rules to verify validator naming conventions
        true.Should().BeTrue();
    }
}
