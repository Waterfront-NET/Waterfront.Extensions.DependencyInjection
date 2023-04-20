using Microsoft.Extensions.DependencyInjection;
using Waterfront.Core.Configuration.Tokens;

namespace Waterfront.Extensions.DependencyInjection;

public static class WaterfrontExtensions
{
    public static IWaterfrontBuilder AddWaterfront(this IServiceCollection services)
    {
        return new WaterfrontBuilder(services);
    }

    public static IServiceCollection AddWaterfront(
        this IServiceCollection services,
        Action<IWaterfrontBuilder> configureWaterfront
    )
    {
        configureWaterfront(services.AddWaterfront());
        return services;
    }

    public static TokenOptions SetIssuer(this TokenOptions self, string issuer)
    {
        self.Issuer = issuer;
        return self;
    }

    public static TokenOptions SetLifetime(this TokenOptions self, TimeSpan lifetime)
    {
        self.Lifetime = lifetime;
        return self;
    }

    public static TokenOptions SetLifetime(this TokenOptions self, int lifetimeSeconds)
    {
        return self.SetLifetime(TimeSpan.FromSeconds(lifetimeSeconds));
    }
}
