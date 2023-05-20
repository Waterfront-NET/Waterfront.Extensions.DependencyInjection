using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Core.Authentication;
using Waterfront.Core.Authorization;

namespace Waterfront.Extensions.DependencyInjection;

public partial class WaterfrontBuilder
{
    public IWaterfrontBuilder WithAuthentication<TService>() where TService : class, IAclAuthenticationService
    {
        Services.AddScoped<IAclAuthenticationService, TService>();
        return this;
    }

    public IWaterfrontBuilder WithAuthentication<TService, TOptions>(string name, Action<TOptions> configureOptions)
        where TService : AclAuthenticationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthenticationService, TService>(
            sp =>
            {
                TOptions options = sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Get(name);
                TService service = ActivatorUtilities.CreateInstance<TService>(sp, options);

                return service;
            }
        );
        Services.AddSingleton<IConfigureOptions<TOptions>>(new ConfigureNamedOptions<TOptions>(name, configureOptions));
        return this;
    }

    public IWaterfrontBuilder WithAuthentication<TService, TOptions>(Action<TOptions> configureOptions)
        where TService : AclAuthenticationServiceBase<TOptions> where TOptions : class =>
        WithAuthentication<TService, TOptions>(Options.DefaultName, configureOptions);

    public IWaterfrontBuilder WithAuthentication<TService, TOptions>(
        string name,
        Action<IServiceProvider, TOptions> configureOptions
    ) where TService : AclAuthenticationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthenticationService, TService>(
            sp =>
            {
                TOptions options = sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Get(name);
                TService service = ActivatorUtilities.CreateInstance<TService>(sp, options);

                return service;
            }
        );
        Services.AddSingleton<IConfigureOptions<TOptions>>(
            sp => new ConfigureNamedOptions<TOptions>(name, options => configureOptions(sp, options))
        );
        return this;
    }
}
