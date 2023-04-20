using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authorization;
using Waterfront.Core.Authorization;

namespace Waterfront.Extensions.DependencyInjection;

public partial class WaterfrontBuilder
{
    public IWaterfrontBuilder WithAuthorization<TAuthz>()
    where TAuthz : class, IAclAuthorizationService
    {
        Services.AddScoped<IAclAuthorizationService, TAuthz>();
        return this;
    }

    public IWaterfrontBuilder WithAuthorization<TAuthz, TOptions>(Action<TOptions> configureOptions)
    where TAuthz : AclAuthorizationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthorizationService, TAuthz>();
        Services.AddTransient<IConfigureOptions<TOptions>>(
            _ => new ConfigureOptions<TOptions>(configureOptions)
        );
        return this;
    }

    public IWaterfrontBuilder WithAuthorization<TAuthz, TOptions>(
        Action<TOptions, IServiceProvider> configureOptions
    ) where TAuthz : AclAuthorizationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthorizationService, TAuthz>();
        Services.AddTransient<IConfigureOptions<TOptions>>(
            sp => new ConfigureOptions<TOptions>(opt => configureOptions(opt, sp))
        );
        return this;
    }

    public IWaterfrontBuilder WithAuthorization<TAuthz, TOptions>(
        Action<TOptions, IConfiguration> configureOptions
    ) where TAuthz : AclAuthorizationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthorizationService, TAuthz>();
        Services.AddTransient<IConfigureOptions<TOptions>>(
            sp => new ConfigureOptions<TOptions>(
                opt => configureOptions(opt, sp.GetRequiredService<IConfiguration>())
            )
        );
        return this;
    }

    public IWaterfrontBuilder WithAuthorization<TAuthz, TOptions, TDependency>(
        Action<TOptions, TDependency> configureOptions
    ) where TAuthz : AclAuthorizationServiceBase<TOptions>
      where TOptions : class
      where TDependency : notnull
    {
        Services.AddScoped<IAclAuthorizationService, TAuthz>();
        Services.AddTransient<IConfigureOptions<TOptions>>(
            sp => new ConfigureOptions<TOptions>(
                opt => configureOptions(opt, sp.GetRequiredService<TDependency>())
            )
        );
        return this;
    }
}
