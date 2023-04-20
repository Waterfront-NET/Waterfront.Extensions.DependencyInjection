using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Waterfront.Core.Configuration.Tokens;

namespace Waterfront.Extensions.DependencyInjection;

public partial class WaterfrontBuilder
{
    public IWaterfrontBuilder ConfigureTokens(Action<TokenOptions> configure)
    {
        Services.Replace(
            ServiceDescriptor.Transient<IConfigureOptions<TokenOptions>>(
                _ => new ConfigureOptions<TokenOptions>(configure)
            )
        );

        return this;
    }

    public IWaterfrontBuilder ConfigureTokens(Action<TokenOptions, IServiceProvider> configure)
    {
        Services.Replace(
            ServiceDescriptor.Transient<IConfigureOptions<TokenOptions>>(
                sp => new ConfigureOptions<TokenOptions>(opt => configure(opt, sp))
            )
        );
        return this;
    }

    public IWaterfrontBuilder ConfigureTokens(Action<TokenOptions, IConfiguration> configure)
    {
        Services.Replace(
            ServiceDescriptor.Transient<IConfigureOptions<TokenOptions>>(
                sp => new ConfigureOptions<TokenOptions>(
                    opt => configure(opt, sp.GetRequiredService<IConfiguration>())
                )
            )
        );
        return this;
    }

    public IWaterfrontBuilder ConfigureTokens<TDependency>(
        Action<TokenOptions, TDependency> configure
    ) where TDependency : notnull
    {
        Services.Replace(
            ServiceDescriptor.Transient<IConfigureOptions<TokenOptions>>(
                sp => new ConfigureOptions<TokenOptions>(
                    opt => configure(opt, sp.GetRequiredService<TDependency>())
                )
            )
        );
        return this;
    }
}
