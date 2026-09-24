using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Naitrust.Api.Configuration;
using Xunit;

namespace Naitrust.UnitTests.Security;

public class CorsConfigurationTests
{
    [Theory]
    [InlineData("https://naiapp-web-lhxm.vercel.app", true)]
    [InlineData("https://existing.example", true)]
    [InlineData("https://unrelated.vercel.app", false)]
    [InlineData("https://naiapp-web-lhxm.vercel.app.attacker.example", false)]
    public async Task LoginPreflight_OnlyAllowsExplicitOrigins(string origin, bool allowed)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Cors:AllowedOrigins:0"] = "https://existing.example",
            ["Cors:AdditionalAllowedOrigins:0"] = "https://naiapp-web-lhxm.vercel.app"
        }).Build();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddCorsPolicy(configuration);
        using var provider = services.BuildServiceProvider();
        var context = new DefaultHttpContext();
        context.Request.Method = "OPTIONS";
        context.Request.Headers.Origin = origin;
        context.Request.Headers.AccessControlRequestMethod = "POST";
        context.Request.Headers.AccessControlRequestHeaders = "content-type,authorization";
        var policy = await provider.GetRequiredService<ICorsPolicyProvider>().GetPolicyAsync(context, "NaitrustCorsPolicy");
        Assert.NotNull(policy);
        var result = provider.GetRequiredService<ICorsService>().EvaluatePolicy(context, policy);
        Assert.Equal(allowed, result.IsOriginAllowed);
        if (allowed)
        {
            Assert.Equal(origin, result.AllowedOrigin);
            Assert.Contains("POST", result.AllowedMethods);
            Assert.Contains("authorization", result.AllowedHeaders);
        }
    }
}
