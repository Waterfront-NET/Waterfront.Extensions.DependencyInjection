using System.Reflection;
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
        /*Services.AddScoped<IAclAuthenticationService, TAuthn>();
        Services.AddTransient<IConfigureOptions<TOptions>>(
            _ => new ConfigureOptions<TOptions>(configureOptions)
        );
        return this;*/

        return WithAuthentication<TAuthn, TOptions>(Options.DefaultName, configureOptions);
    }

    public IWaterfrontBuilder WithAuthentication<TAuthn, TOptions>(
        string name,
        Action<TOptions> configureOptions
    ) where TAuthn : AclAuthenticationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthenticationService, TAuthn>(
            sp => {
                TOptions? options = sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Get(name);

                Type            type        = typeof(TAuthn);
                ConstructorInfo constructor = type.GetConstructors(BindingFlags.Public).First();

                ParameterInfo[] parameters = constructor.GetParameters();

                Dictionary<ParameterInfo, object> dict = new Dictionary<ParameterInfo, object>();

                foreach ( ParameterInfo parameter in parameters )
                {
                    if ( parameter.ParameterType == typeof(TOptions) )
                    {
                        dict[parameter] = options;
                    }
                    else
                    {
                        dict[parameter] = sp.GetRequiredService(parameter.ParameterType);
                    }
                }

                TAuthn instance = (TAuthn) Activator.CreateInstance(type, dict.Values)!;

                return instance;
            }
        );
    }

    public IWaterfrontBuilder WithAuthentication<TAuthn, TOptions>(
        Action<TOptions, IServiceProvider> configureOptions
    ) where TAuthn : AclAuthenticationServiceBase<TOptions> where TOptions : class
    {
        Services.AddScoped<IAclAuthenticationService, TAuthn>(
            sp => {
                string? optName = Options.DefaultName;
            }
        );
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
