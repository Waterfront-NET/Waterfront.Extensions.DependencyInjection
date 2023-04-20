using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Core.Authentication;

namespace Waterfront.Extensions.DependencyInjection;

public partial class WaterfrontBuilder
{
    public IWaterfrontBuilder WithAuthentication<TAuthn>()
    where TAuthn : class, IAclAuthenticationService
    {
        Services.AddScoped<IAclAuthenticationService, TAuthn>();
        return this;
    }

    public IWaterfrontBuilder WithAuthentication<TAuthn, TOptions>(
        Action<TOptions> configureOptions
    ) where TAuthn : AclAuthenticationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthenticationService, TAuthn>();
        Services.AddTransient<IConfigureOptions<TOptions>>(
            _ => new ConfigureOptions<TOptions>(configureOptions)
        );
        return this;
    }

    public IWaterfrontBuilder WithAuthentication<TAuthn, TOptions>(
        Action<TOptions, IServiceProvider> configureOptions
    ) where TAuthn : AclAuthenticationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthenticationService, TAuthn>();
        Services.AddTransient<IConfigureOptions<TOptions>>(
            sp => new ConfigureOptions<TOptions>(opt => configureOptions(opt, sp))
        );
        return this;
    }

    public IWaterfrontBuilder WithAuthentication<TAuthn, TOptions>(
        Action<TOptions, IConfiguration> configureOptions
    ) where TAuthn : AclAuthenticationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthenticationService, TAuthn>();
        Services.AddTransient<IConfigureOptions<TOptions>>(
            sp => new ConfigureOptions<TOptions>(
                opt => configureOptions(opt, sp.GetRequiredService<IConfiguration>())
            )
        );
        return this;
    }

    public IWaterfrontBuilder WithAuthentication<TAuthn, TOptions, TDependency>(
        Action<TOptions, TDependency> configureOptions
    ) where TAuthn : AclAuthenticationServiceBase<TOptions>
      where TOptions : class
      where TDependency : notnull
    {
        Services.AddScoped<IAclAuthenticationService, TAuthn>();
        Services.AddTransient<IConfigureOptions<TOptions>>(
            sp => new ConfigureOptions<TOptions>(
                opt => configureOptions(opt, sp.GetRequiredService<TDependency>())
            )
        );
        return this;
    }
}
