using Xunit;

namespace Naitrust.IntegrationTests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    public Task InitializeAsync() => Task.CompletedTask; // TODO: Setup test database
    public Task DisposeAsync() => Task.CompletedTask; // TODO: Teardown test database
}
