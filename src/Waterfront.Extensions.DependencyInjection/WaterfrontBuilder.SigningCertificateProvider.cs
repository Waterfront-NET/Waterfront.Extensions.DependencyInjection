using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Waterfront.Common.Tokens.Signing.CertificateProviders;
using Waterfront.Core.Tokens.Signing.CertificateProviders;

namespace Waterfront.Extensions.DependencyInjection;

public partial class WaterfrontBuilder
{
    public IWaterfrontBuilder WithSigningCertificateProvider<TProvider>()
    where TProvider : class, ISigningCertificateProvider
    {
        Services.Replace(ServiceDescriptor.Singleton<ISigningCertificateProvider, TProvider>());
        return this;
    }

    public IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions>(
        Action<TOptions> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class
    {
        Services.Replace(ServiceDescriptor.Singleton<ISigningCertificateProvider, TProvider>());
        Services.Replace(
            ServiceDescriptor.Transient<IConfigureOptions<TOptions>>(
                _ => new ConfigureOptions<TOptions>(configureOptions)
            )
        );
        return this;
    }

    public IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions>(
        Action<TOptions, IServiceProvider> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class
    {
        Services.Replace(ServiceDescriptor.Singleton<ISigningCertificateProvider, TProvider>());
        Services.Replace(
            ServiceDescriptor.Transient<IConfigureOptions<TOptions>>(
                sp => new ConfigureOptions<TOptions>(opt => configureOptions(opt, sp))
            )
        );
        return this;
    }

    public IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions>(
        Action<TOptions, IConfiguration> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class
    {
        Services.Replace(ServiceDescriptor.Singleton<ISigningCertificateProvider, TProvider>());
        Services.Replace(
            ServiceDescriptor.Transient<IConfigureOptions<TOptions>>(
                sp => new ConfigureOptions<TOptions>(
                    opt => configureOptions(opt, sp.GetRequiredService<IConfiguration>())
                )
            )
        );
        return this;
    }

    public IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions, TDependency>(
        Action<TOptions, TDependency> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions>
      where TOptions : class
      where TDependency : notnull
    {
        Services.Replace(ServiceDescriptor.Singleton<ISigningCertificateProvider, TProvider>());
        Services.Replace(
            ServiceDescriptor.Transient<IConfigureOptions<TOptions>>(
                sp => new ConfigureOptions<TOptions>(
                    opt => configureOptions(opt, sp.GetRequiredService<TDependency>())
                )
            )
        );
        return this;
    }
}
