using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authorization;
using Waterfront.Core.Authorization;

namespace Waterfront.Extensions.DependencyInjection;

public partial class WaterfrontBuilder
{
    public IWaterfrontBuilder WithAuthorization<TService>() where TService : class, IAclAuthorizationService
    {
        Services.AddScoped<IAclAuthorizationService, TService>();
        return this;
    }

    public IWaterfrontBuilder WithAuthorization<TService, TOptions>(string name, Action<TOptions> configureOptions)
        where TService : AclAuthorizationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthorizationService, TService>(
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

    public IWaterfrontBuilder WithAuthorization<TService, TOptions>(Action<TOptions> configureOptions)
        where TService : AclAuthorizationServiceBase<TOptions> where TOptions : class =>
        WithAuthorization<TService, TOptions>(Options.DefaultName, configureOptions);

    public IWaterfrontBuilder WithAuthorization<TService, TOptions>(
        string name,
        Action<IServiceProvider, TOptions> configureOptions
    ) where TService : AclAuthorizationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthorizationService, TService>(
            sp =>
            {
                TOptions options = sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Get(name);
                TService service = ActivatorUtilities.CreateInstance<TService>(sp, options);

                return service;
            });
        Services.AddSingleton<IConfigureOptions<TOptions>>(
            sp => new ConfigureNamedOptions<TOptions>(name, options => configureOptions(sp, options))
        );
        return this;
    }
}
