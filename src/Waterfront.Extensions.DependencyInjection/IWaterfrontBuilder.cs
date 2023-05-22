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

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider>() where TProvider : class, ISigningCertificateProvider;

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions>(Action<TOptions> configureOptions)
        where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions>(
        Action<TOptions, IServiceProvider> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions>(
        Action<TOptions, IConfiguration> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithSigningCertificateProvider<TProvider, TOptions, TDependency>(
        Action<TOptions, TDependency> configureOptions
    ) where TProvider : SigningCertificateProviderBase<TOptions> where TOptions : class where TDependency : notnull;

    IWaterfrontBuilder WithTokenEncoder<TEncoder>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TEncoder : class, ITokenEncoder;

    IWaterfrontBuilder WithTokenDefinitionService<TService>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TService : class, ITokenDefinitionService;

    IWaterfrontBuilder WithAuthentication<TService>() where TService : class, IAclAuthenticationService;

    IWaterfrontBuilder WithAuthentication<TService, TOptions>(string name, Action<TOptions> configureOptions)
        where TService : AclAuthenticationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthentication<TService, TOptions>(Action<TOptions> configureOptions)
        where TService : AclAuthenticationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthentication<TService, TOptions>(
        string name,
        Action<IServiceProvider, TOptions> configureOptions
    ) where TService : AclAuthenticationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthorization<TService>() where TService : class, IAclAuthorizationService;

    IWaterfrontBuilder WithAuthorization<TService, TOptions>(string name, Action<TOptions> configureOptions)
        where TService : AclAuthorizationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthorization<TService, TOptions>(Action<TOptions> configureOptions)
        where TService : AclAuthorizationServiceBase<TOptions> where TOptions : class;

    IWaterfrontBuilder WithAuthorization<TService, TOptions>(
        string name,
        Action<IServiceProvider, TOptions> configureOptions
    ) where TService : AclAuthorizationServiceBase<TOptions> where TOptions : class;
}
