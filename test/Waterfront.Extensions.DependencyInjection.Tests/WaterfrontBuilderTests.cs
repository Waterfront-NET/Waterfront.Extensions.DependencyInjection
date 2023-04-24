using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Waterfront.Core.Configuration.Tokens;

namespace Waterfront.Extensions.DependencyInjection.Tests;

[TestClass]
public class WaterfrontBuilderTests
{
    private static IServiceCollection GetServiceCollection()
    {
        return new ServiceCollection();
    }

    [TestMethod]
    public void TestDefaultCreation()
    {
        IServiceCollection sc = GetServiceCollection();
        new WaterfrontBuilder(sc);
        ServiceProvider sp = sc.BuildServiceProvider();

        IOptions<TokenOptions>? options = sp.GetService<IOptions<TokenOptions>>();
        options.Should().NotBeNull();
        options.Value.Lifetime.Should().Be(TimeSpan.FromSeconds(60));
        options.Value.Issuer.Should().Be("Waterfront");
    }

    [TestMethod]
    public void TestExtCreation()
    {
        IServiceCollection sc = GetServiceCollection();
        sc.AddWaterfront();
        ServiceProvider sp = sc.BuildServiceProvider();

        IOptions<TokenOptions>? options = sp.GetService<IOptions<TokenOptions>>();
        options.Should().NotBeNull();
        options.Value.Lifetime.Should().Be(TimeSpan.FromSeconds(60));
        options.Value.Issuer.Should().Be("Waterfront");
    }

    [TestMethod]
    public void TestConfigureOptions()
    {
        IServiceCollection sc = GetServiceCollection();
        sc.AddWaterfront()
          .ConfigureTokens(tokens => tokens.SetLifetime(120).SetIssuer("localhost"));
        ServiceProvider sp = sc.BuildServiceProvider();

        IOptions<TokenOptions>? options = sp.GetService<IOptions<TokenOptions>>();
        options.Should().NotBeNull();
        options.Value.Lifetime.Should().Be(TimeSpan.FromSeconds(120));
        options.Value.Issuer.Should().Be("localhost");
    }
}
