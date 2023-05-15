using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Waterfront.Common.Authentication;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens.Definition;
using Waterfront.Common.Tokens.Encoding;
using Waterfront.Common.Tokens.Signing.CertificateProviders;
using Waterfront.Core.Authentication;
using Waterfront.Core.Authorization;
using Waterfront.Core.Configuration.Tokens;
using Waterfront.Core.Tokens.Signing.CertificateProviders;

namespace Waterfront.Extensions.DependencyInjection;

public interface IWaterfrontBuilder
{
    IServiceCollection Services { get; }

    IWaterfrontBuilder ConfigureTokens(Action<TokenOptions> configure);
    IWaterfrontBuilder ConfigureTokens(Action<TokenOptions, IServiceProvider> configure);
    IWaterfrontBuilder ConfigureTokens(Action<TokenOptions, IConfiguration> configure);

    IWaterfrontBuilder ConfigureTokens<TDependency>(Action<TokenOptions, TDependency> configure)
    where TDependency : notnull;

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider>()
    where TProvider : class, ISigningCertificateProvider;

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions>(
        Action<TOptions> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions>(
        Action<TOptions, IServiceProvider> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions>(
        Action<TOptions, IConfiguration> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions, TDependency>(
        Action<TOptions, TDependency> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions>
      where TOptions : class
      where TDependency : notnull;

    IWaterfrontBuilder WithTokenEncoder<TEncoder>(
        ServiceLifetime lifetime = ServiceLifetime.Singleton
    ) where TEncoder : class, ITokenEncoder;

    IWaterfrontBuilder WithTokenDefinitionService<TService>(
        ServiceLifetime lifetime = ServiceLifetime.Scoped
    ) where TService : class, ITokenDefinitionService;

    /*IWaterfrontBuilder WithAuthentication<TAuthn>() where TAuthn : class, IAclAuthenticationService;

    IWaterfrontBuilder WithAuthentication<TAuthn, TOptions>(Action<TOptions> configureOptions)
    where TAuthn : AclAuthenticationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthentication<TAuthn, TOptions>(
        Action<TOptions, IServiceProvider> configureOptions
    ) where TAuthn : AclAuthenticationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthentication<TAuthn, TOptions>(
        Action<TOptions, IConfiguration> configureOptions
    ) where TAuthn : AclAuthenticationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthentication<TAuthn, TOptions, TDependency>(
        Action<TOptions, TDependency> configureOptions
    ) where TAuthn : AclAuthenticationServiceBase<TOptions>
      where TOptions : class
      where TDependency : notnull;

    IWaterfrontBuilder WithAuthorization<TAuthz>() where TAuthz : class, IAclAuthorizationService;

    IWaterfrontBuilder WithAuthorization<TAuthz, TOptions>(Action<TOptions> configureOptions)
    where TAuthz : AclAuthorizationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthorization<TAuthz, TOptions>(
        Action<TOptions, IServiceProvider> configureOptions
    ) where TAuthz : AclAuthorizationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthorization<TAuthz, TOptions>(
        Action<TOptions, IConfiguration> configureOptions
    ) where TAuthz : AclAuthorizationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthorization<TAuthz, TOptions, TDependency>(
        Action<TOptions, TDependency> configureOptions
    ) where TAuthz : AclAuthorizationServiceBase<TOptions>
      where TOptions : class
      where TDependency : notnull;*/
}
